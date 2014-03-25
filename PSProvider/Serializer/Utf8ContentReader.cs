using System.Collections;
using System.IO;
using System.Management.Automation.Provider;
using System.Text;

namespace Zookeeper.PSProvider.Serializer
{
    public class Utf8ContentReader : IContentReader
    {
        private readonly byte[] _data;
        private bool _wasRead;

        public Utf8ContentReader(byte[] data)
        {
            this._data = data;
        }

        public void Dispose()
        {
        }

        public IList Read(long readCount)
        {
            if (this._wasRead)
            {
                return new ArrayList();
            }

            var value = Encoding.UTF8.GetString(this._data);

            this._wasRead = true;

            return new ArrayList() { value };
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            if (offset < 0 && (origin == SeekOrigin.Current) || origin == SeekOrigin.End)
            {
                this._wasRead = false;
            }

            if (origin == SeekOrigin.Begin && offset > 0)
            {
                this._wasRead = true;
            }
        }

        public void Close()
        {
        }
    }
}