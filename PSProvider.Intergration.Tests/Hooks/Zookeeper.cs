using System.Linq;
using TechTalk.SpecFlow;

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
            this.CleanZookeeper();
            if (this.context.ZookeeperClient != null)
            {
                this.context.ZookeeperClient.Stop();
            }
        }

        private void CleanZookeeper()
        {
            if( this.context.ZookeeperClient == null )
            {
                return;
            }
            var items = this.context.ZookeeperClient.GetChildren("/", false).Result.Where(s => s != "zookeeper").ToArray();

            foreach (var item in items)
            {
                this.Remove("/" + item);
            }
        }

        private void Remove(string item)
        {
            var subItems = this.context.ZookeeperClient.GetChildren2("/", false).Result;
            var node = this.context.ZookeeperClient.Exists("/"+subItems.Children[0], false).Result;
            var node2 = this.context.ZookeeperClient.Exists("/"+subItems.Children[1], false).Result;
            foreach (var subItem in this.context.ZookeeperClient.GetChildren(item, false).Result)
            {
                this.Remove(item + "/" + subItem);
            }

            this.context.ZookeeperClient.Delete(item, -1).Wait();
        }
    }
}
