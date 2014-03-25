using System;
using Sodao.Zookeeper;

namespace Zookeeper.PSProvider
{
    public class ZookeeperConnectionFactory
    {
        public static IZookClient CreateZookeeper(string connectionString)
        {
            return new ZookClient("/", connectionString, TimeSpan.FromMinutes(10), new LogWatcher());
        }
    }
}