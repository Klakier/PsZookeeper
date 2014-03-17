using System;
using ZooKeeperNet;

namespace Zookeeper.PSProvider
{
    public class ZookeeperConnectionFactory
    {
        public static IZooKeeper CreateZookeeper(string connectionString)
        {
            return new ZooKeeperNet.ZooKeeper(connectionString, TimeSpan.FromMinutes(10), new LogWatcher());
        }
    }
}