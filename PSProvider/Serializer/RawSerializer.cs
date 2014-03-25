using System;

namespace Zookeeper.PSProvider.Serializer
{
    internal class RawSerializer : ISerializer
    {
        public byte[] Serialize(object item)
        {
            var raw = item as byte[];
            if( raw == null )
            {
                throw new ArgumentException("Raw serializer support only array of bytes");
            }

            return raw;
        }
    }
}