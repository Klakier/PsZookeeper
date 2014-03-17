using System;
using System.Linq;
using Org.Apache.Zookeeper.Data;
using TechTalk.SpecFlow;

namespace Zookeeper.PSProvider.Intergration.Tests.Given
{
    [Binding]
    public class Zookeeper
    {
        private readonly Context _context;

        public Zookeeper(Context context)
        {
            this._context = context;
        }

        [Given(@"I have zookeeper client connected to '(.*)'")]
        public void GivenIHaveZookeeperClientConnectedTo(string addresss)
        {
            this._context.ZookeeperClient = new ZooKeeperNet.ZooKeeper(addresss, TimeSpan.FromSeconds(10), new LogWatcher());
        }

        [Given(@"I clear zookeeper configuration")]
        public void GivenIClearZookeeperConfiguration()
        {
            var childs = this._context.ZookeeperClient.GetChildren(ZookeeperPsDriverInfo.Separator, false);
            foreach (var subElement in childs.Select(s => ZookeeperPsDriverInfo.Separator + s).Where(s => s != ZookeeperPsDriverInfo.SystemFolder))
            {
                this.Remove(subElement);
            }
        }

        public void Remove(string path)
        {
            var state = this._context.ZookeeperClient.Exists(path, false);
            if (state == null)
            {
                return;
            }

            if (state.NumChildren != 0)
            {
                foreach (var element in this._context.ZookeeperClient.GetChildren(path, false))
                {
                    this.Remove(path + "/" + element);
                }
            }

            this._context.ZookeeperClient.Delete(path, state.Version);
        }
    }
}