using System.Collections.Generic;
using System.Management.Automation;
using ZooKeeperNet;

namespace Zookeeper.PSProvider.Intergration.Tests
{
    public class Context
    {
        public Context()
        {
            this.RegisterdAssemblies = new List<string>();
        }

        public PowerShell PowershellHost { get; set; }
        public ICollection<string> RegisterdAssemblies { get; set; }
        public ZooKeeper ZookeeperClient { get; set; }
    }
}