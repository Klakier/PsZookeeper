using System;
using Sodao.Zookeeper.Data;
using Zookeeper.PSProvider.Paths;

namespace Zookeeper.PSProvider.ItemsRepresntation
{
    public class LongNodeRepresentation
    {
        private string path;
        private GetDataResponse data;

        public LongNodeRepresentation(
                GetDataResponse data,
                ZookeeperPsDriverInfo driverInfo,
                string path )
        {
            this.path = path;
            this.data = data;

            this.Name = ZookeeperPath.GetItemName( path );
            this.FullName = PsPath.FromZookeeperPath( driverInfo, path );
        }

        public string Name  { get ; private set; }

        public string FullName { get; private set; }

        public byte[] Data { get { return  data.Data; } }

        public int NumberOfChildren { get { return data.Stat.NumChildren; } }

        public int Version { get { return data.Stat.Version; } }
    }
}
