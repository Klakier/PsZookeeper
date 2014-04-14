using System.Linq;
using TechTalk.SpecFlow;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Hooks
{
    [Binding]
    public class Zookeeper
    {
        private readonly Context context;

        public Zookeeper(Context context)
        {
            this.context = context;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            using( var zk = new ZookeeperHelpers() )
            {
                zk.CleanZookeeper();
            }

            if (this.context.ZookeeperClient != null)
            {
                this.context.ZookeeperClient.Stop();
            }
        }
    }
}
