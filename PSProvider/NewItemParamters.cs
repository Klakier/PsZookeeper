using System.Management.Automation;
using Sodao.Zookeeper.Data;
using Zookeeper.PSProvider.Serializer;

namespace Zookeeper.PSProvider
{
    public class NewItemParamters
    {
        public NewItemParamters()
        {
            this.CreateMode = CreateModes.Persistent;
            this.Encoding = EncodingType.Utf8;
        }

        [Parameter]
        public CreateMode CreateMode { get; set; }

        [Parameter]
        public EncodingType Encoding { get; set; }
    }
}