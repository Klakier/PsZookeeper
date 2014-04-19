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
            return string.Format(
                    "Name: {0}, Version: {2}, NumberOfChildren: {3}, Data: {4}", 
                    this.Name,
                    this.Version,
                    this.NumberOfChildren,
                    this.Data);
        }
    }
}
