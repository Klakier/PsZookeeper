using System.Management.Automation;
using Zookeeper.PSProvider.Serializer;

namespace Zookeeper.PSProvider
{
    public class GetContentDynamicParameters
    {
        public GetContentDynamicParameters()
        {
            Encoding = EncodingType.Utf8;
            Version = Zookeeper.AnyVersion;
        }

        [Parameter]
        public EncodingType Encoding { get; set; }

        [Parameter]
        public int Version { get; set; }

    }
}