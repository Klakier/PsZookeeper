using System.Collections;
using System.IO;
using System.Management.Automation.Provider;

namespace Zookeeper.PSProvider
{
    public class RawContentReader : IContentReader
    {
        private readonly MemoryStream data;

        public RawContentReader( byte[] data)
        {
            this.data = new MemoryStream(data);
        }

        public void Dispose()
        {
        }

        public IList Read(long readCount)
        {
            if (this.data.Position == this.data.Length)
            {
                return new ArrayList();
            }

            var buffer = new byte[readCount];
            this.data.Read(buffer, 0, (int)readCount);

            return new ArrayList(buffer);
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            this.data.Seek(offset, origin);
        }

        public void Close()
        {
            this.data.Close();
        }
    }
}