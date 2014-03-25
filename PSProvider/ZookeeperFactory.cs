namespace Zookeeper.PSProvider
{
    public static class ZookeeperFactory
    {
        public static IZookeeper Create(Configuration config)
        {
            return new NormalizePathZookeeperDecorator(
                new Zookeeper(config));
        }
    }
}