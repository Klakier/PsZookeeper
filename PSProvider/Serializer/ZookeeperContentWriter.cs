using System;
using System.Collections;
using System.IO;
using System.Management.Automation.Provider;

namespace Zookeeper.PSProvider.Serializer
{
    public class ZookeeperContentWriter : IContentWriter
    {
        public ZookeeperContentWriter(Action<byte[]> save)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IList Write(IList content)
        {
            throw new NotImplementedException();
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}