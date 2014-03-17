using System;
using System.Management.Automation;
using ZooKeeperNet;

namespace Zookeeper.PSProvider
{
    public class ZookeeperPsDriverInfo : PSDriveInfo
    {
        public const string Separator = @"/";
        public static string SystemFolder = Separator + "zookeeper";

        public const string WildCard = "*";

        public ZookeeperPsDriverInfo(PSDriveInfo driveInfo)
            : base(driveInfo)
        {
            this.Connection = ZookeeperConnectionFactory.CreateZookeeper(driveInfo.Root);
        }

        public IZooKeeper Connection { get; private set; }

        public void Reconnect()
        {
            this.Connection.Dispose();
            this.Connection = ZookeeperConnectionFactory.CreateZookeeper(this.Root);
        }
        public void Execute(Action<IZooKeeper> action)
        {
            try
            {
                action(this.Connection);
            }
            catch (KeeperException.SessionExpiredException)
            {
                this.Reconnect();
                action(this.Connection);
            }
        }

        public T Execute<T>( Func<IZooKeeper, T> action)
        {
            try
            {
                return action(this.Connection);
            }
            catch (KeeperException.SessionExpiredException)
            {
                this.Reconnect();
                return action(this.Connection);
            }
        }
    }
}