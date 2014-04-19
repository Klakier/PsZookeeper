using System.Management.Automation;

namespace Zookeeper.PSProvider
{
    public class ZookeeperPsDriverInfo : PSDriveInfo
    {
        private readonly IZookeeper zookeeper;

        public ZookeeperPsDriverInfo(PSDriveInfo driveInfo, IZookeeper zookeeper)
            : base(driveInfo)
        {
            this.zookeeper = zookeeper;
        }

        public IZookeeper Zookeeper { get { return this.zookeeper; } }
    }
}
