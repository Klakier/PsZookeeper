using System.Collections.Generic;
using Sodao.Zookeeper.Data;
using Zookeeper.PSProvider.Paths;

namespace Zookeeper.PSProvider
{
    public class NormalizePathZookeeperDecorator : IZookeeper
    {
        private readonly IZookeeper innerZookeeper;

        public NormalizePathZookeeperDecorator(IZookeeper innerZookeeper)
        {
            this.innerZookeeper = innerZookeeper;
        }

        public IEnumerable<string> GetChildren(string path)
        {
            var normalizedPath = ZookeeperPath.Normalize(path);

            return this.innerZookeeper.GetChildren(normalizedPath);
        }

        public bool PathExist(string path)
        {
            var normalizedPath = ZookeeperPath.Normalize(path);

            return this.innerZookeeper.PathExist(normalizedPath);
        }

        public void CreateItem(string path, byte[] data, CreateMode createMode)
        {
            var normalizedPath = ZookeeperPath.Normalize(path);

            this.innerZookeeper.CreateItem(normalizedPath, data, createMode);
        }

        public void Remove(string path, bool recurse)
        {
            var normalizedPath = ZookeeperPath.Normalize(path);

            this.innerZookeeper.Remove(normalizedPath, recurse); 
        }

        public GetDataResponse GetItem(string path)
        {
            var normalizedPath = ZookeeperPath.Normalize(path);
            return this.innerZookeeper.GetItem(normalizedPath);
        }

        public Stat GetStat(string path)
        {
            return this.innerZookeeper.GetStat(ZookeeperPath.Normalize(path));
        }

        public void SetData(string path, byte[] data, int version)
        {
            this.innerZookeeper.SetData(ZookeeperPath.Normalize(path), data, version);
        }
    }
}
