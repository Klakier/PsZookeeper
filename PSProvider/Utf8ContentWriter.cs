using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Management.Automation.Provider;
using System.Text;

namespace Zookeeper.PSProvider
{
    public class Utf8ContentWriter : IContentWriter
    {
        private readonly Action<byte[]> saveData;

        public Utf8ContentWriter(Action<byte[]> saveData)
        {
            this.saveData = saveData;
        }

        public void Dispose()
        {
        }

        public IList Write(IList content)
        {
            var newData = content.OfType<string>().ToArray();
            if (newData.Length != content.Count)
            {
                throw new ArgumentException("Utf8 encoding accept only strings");
            }

            var stringData = string.Join(Environment.NewLine, newData);
            var binaryData = Encoding.UTF8.GetBytes(stringData);

            this.saveData(binaryData);

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