namespace Zookeeper.PSProvider
{
    public class ZookeeperItem
    {
        public ZookeeperItem( string name, string fullName )
        {
            this.Name = name;
            this.FullName = fullName;
        }

        public string Name { get; private set; }

        public string FullName { get; private set; }
    }
}
