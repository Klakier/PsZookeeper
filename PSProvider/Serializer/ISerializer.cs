namespace Zookeeper.PSProvider.Serializer
{
    internal interface ISerializer
    {
        byte[] Serialize(object item);
    }
}