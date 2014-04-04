using System;
using System.Collections.Generic;
using System.Linq;
using Sodao.Zookeeper;
using Sodao.Zookeeper.Data;

namespace Zookeeper.PSProvider
{
    public class Zookeeper : IZookeeper
    {
        public const int AnyVersion = -1;
        private readonly ZookClient client;

        public Zookeeper(Configuration configuration)
        {
            var zookeeperRoot = configuration.Root == ZookeeperPath.Separator ? string.Empty : configuration.Root;
            this.client = new ZookClient(
                zookeeperRoot,
                configuration.ConnectionString,
                configuration.SessionTimeout);
        }

        public IEnumerable<string> GetChildren(string path)
        {
            return this.client.GetChildren(path, false).Result;
        }

        public bool PathExist(string path)
        {
            return this.client.Exists(path, false).Result != null;
        }

        public void CreateItem(string path, byte[] data, CreateMode createMode)
        {
            this.client.Create(path, data, IDs.OPEN_ACL_UNSAFE, createMode).Wait();
        }

        public void Remove(string path, bool recurse)
        {
            if (!recurse)
            {
                this.client.Delete(path, -1);
            }
            else
            {
                this.RemoveRecurse(path);
            }
        }

        private void RemoveRecurse(string item)
        {
            foreach (var subItem in this.client.GetChildren(item, false).Result)
            {
                this.RemoveRecurse(item + "/" + subItem);
            }

            this.client.Delete(item, -1).Wait();
        }

        public NodeInfo GetItem(string path)
        {
            var data = this.GetData(path);

            return new NodeInfo
            {
                Name = ZookeeperPath.GetItemName(path),
                Data = data.Data,
                NumberOfChildren = data.Stat.NumChildren,
                Version = data.Stat.Version
            };
        }

        public Stat GetStat(string path)
        {
            return this.client.Exists(path, false).Result;
        }

        public GetDataResponse GetData(string path)
        {
            return this.client.GetData(path, false).Result;
        }

        public void SetData(string path, byte[] data, int version)
        {
            this.client.SetData(path, data, version).Wait();
        }
    }
}
