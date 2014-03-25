using System;
using System.Linq;
using Sodao.Zookeeper;
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
            this._context.ZookeeperClient = new ZookClient(string.Empty, addresss, TimeSpan.FromSeconds(10), new LogWatcher());
        }

        [Given(@"I clear zookeeper configuration")]
        public void GivenIClearZookeeperConfiguration()
        {
            var childs = this._context.ZookeeperClient.GetChildren(ZookeeperPath.Separator, false).Result;
            foreach (var subElement in childs.Select(s => ZookeeperPath.Separator + s).Where(s => s != ZookeeperPath.SystemFolder))
            {
                this.Remove(subElement);
            }
        }

        public void Remove(string path)
        {
            var state = this._context.ZookeeperClient.Exists(path, false).Result;
            if (state == null)
            {
                return;
            }

            if (state.NumChildren != 0)
            {
                foreach (var element in this._context.ZookeeperClient.GetChildren(path, false).Result)
                {
                    this.Remove(path + "/" + element);
                }
            }

            this._context.ZookeeperClient.Delete(path, state.Version);
        }
    }
}