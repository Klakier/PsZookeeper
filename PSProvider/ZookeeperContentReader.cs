using System;
using System.Collections;
using System.IO;
using System.Management.Automation.Provider;
using Org.Apache.Zookeeper.Data;

namespace Zookeeper.PSProvider
{
    public class ZookeeperContentReader : IContentReader
    {
        private Stat _stat;
        private readonly MemoryStream _data;

        public ZookeeperContentReader(Stat stat, byte[] data)
        {
            this._stat = stat;
            this._data = new MemoryStream(data);
        }

        public void Dispose()
        {
        }

        public IList Read(long readCount)
        {
            if (this._data.Position == this._data.Length)
            {
                return new ArrayList();
            }

            var buffer = new byte[readCount];
            this._data.Read(buffer, 0, (int)readCount);

            return new ArrayList(buffer);
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            this._data.Seek(offset, origin);
        }

        public void Close()
        {
            this._data.Close();
        }
    }
}