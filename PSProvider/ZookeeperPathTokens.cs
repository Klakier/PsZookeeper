namespace Zookeeper.PSProvider
{
    public class ZookeeperPathTokens
    {
        public ZookeeperPathTokens(bool hasWildCard, string wildCardPattern, string knwonPath)
        {
            this.HasWildCard = hasWildCard;
            this.WildCardPattern = wildCardPattern;
            this.KnwonPath = knwonPath;
        }

        public bool HasWildCard { get; private set; }
        public string KnwonPath { get; private set; }

        public string WildCardPattern { get; private set; }
    }
}