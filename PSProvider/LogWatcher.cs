using System;
using Sodao.Zookeeper;
using Sodao.Zookeeper.Data;

namespace Zookeeper.PSProvider
{
    public class LogWatcher : IWatcher
    {
        public void Process(WatchedEvent zevent)
        {
            Console.WriteLine("Log watche: {0}", zevent.Path);
        }
    }
}
