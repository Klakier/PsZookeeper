using System.Collections.Generic;
using System.Management.Automation;
using Sodao.Zookeeper;

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
        public ZookClient ZookeeperClient { get; set; }
    }
}