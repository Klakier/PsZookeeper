using System.Collections;
using System.IO;
using System.Management.Automation.Provider;
using System.Text;

namespace Zookeeper.PSProvider.Serializer
{
    public class Utf8ContentReader : IContentReader
    {
        private readonly byte[] data;
        private bool _wasRead;

        public Utf8ContentReader(byte[] data)
        {
            this.data = data;
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

            this._wasRead = true;

            if( this.data == null || this.data.Length == 0 )
            {
                return new ArrayList() { string.Empty };
            }

            var value = Encoding.UTF8.GetString(this.data);

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
