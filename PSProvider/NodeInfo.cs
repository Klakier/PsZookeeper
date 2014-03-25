using System;

namespace Zookeeper.PSProvider
{
    public class NodeInfo
    {
        public string Name { get; set; }
        public int Version { get; set; }

        public int NumberOfChildren { get; set; }

        public byte[] Data { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0},Version: {1}, NumberOfChildren: {2}, Data: {3}", Name, Version, NumberOfChildren, Data);
        }
    }
}