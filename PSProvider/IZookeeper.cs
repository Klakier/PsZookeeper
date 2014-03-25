using System.Collections.Generic;
using Sodao.Zookeeper.Data;

namespace Zookeeper.PSProvider
{
    public interface IZookeeper
    {
        IEnumerable<string> GetChildren(string path);
        bool PathExist(string path);
        void CreateItem(string path, byte[] data, CreateMode createMode);
        NodeInfo GetItem(string path);
        Stat GetStat(string path);
        IEnumerable<string> GetChildrenRecurse(string path);
        GetDataResponse GetData(string path);
        void SetData(string path, byte[] data, int version);
    }
}