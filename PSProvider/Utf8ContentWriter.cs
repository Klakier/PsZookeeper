using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Management.Automation.Provider;
using System.Text;
using ZooKeeperNet;

namespace Zookeeper.PSProvider
{
    public class Utf8ContentWriter : IContentWriter
    {
        private readonly ZookeeperPsDriverInfo _connection;
        private readonly string _path;

        public Utf8ContentWriter(ZookeeperPsDriverInfo connection, string path)
        {
            _connection = connection;
            _path = path;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IList Write(IList content)
        {
            var stat = this._connection.Execute(z => z.Exists(this._path, false));

            var newData = content.OfType<string>().ToArray();
            if (newData.Length != content.Count)
            {
                throw new ArgumentException("Utf8 encoding accept only strings");
            }

            var stringData = string.Join(Environment.NewLine, newData);
            var binaryData = Encoding.UTF8.GetBytes(stringData);
            this._connection.Execute(z => z.SetData(this._path, binaryData, stat.Version));

            return new ArrayList()
            {
                binaryData
            };
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
        }
    }
}