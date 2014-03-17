using System;

namespace Zookeeper.PSProvider
{
    public class NodeInfo
    {
        public int Version { get; set; }

        public int NumberOfChildren { get; set; }

        public string Data { get; set; }

        public override string ToString()
        {
            return string.Format("Version: {0}, NumberOfChildren: {1}, Data: {2}", Version, NumberOfChildren, Data);
        }
    }
}