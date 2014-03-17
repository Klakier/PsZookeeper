using System;
using ZooKeeperNet;

namespace Zookeeper.PSProvider
{
    public class LogWatcher : IWatcher
    {
        public void Process(WatchedEvent @event)
        {
            Console.WriteLine("Log watche: {0}", @event.Path);
        }
    }
}