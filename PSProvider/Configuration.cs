using System;

namespace Zookeeper.PSProvider
{
    public class Configuration
    {
        public Configuration()
        {
            this.SessionTimeout = TimeSpan.MaxValue;
            this.ConnectionString = "127.0.0.1:2181";
            this.Root = string.Empty;
        }

        public TimeSpan SessionTimeout { get; set; }
        public string ConnectionString { get; set; }

        public string Root { get; set; }
    }
}