using System;
using System.Text;

namespace Zookeeper.PSProvider.Serializer
{
    internal class Utf8Serializer : ISerializer
    {
        public byte[] Serialize(object item)
        {
            if( item == null )
            {
                return new byte[0];
            }

            return Encoding.UTF8.GetBytes(item.ToString());
        }
    }
}