using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Zookeeper.PSProvider.Intergration.Tests.Tranformations
{
    [Binding]
    public class ItemTransformation
    {
        [StepArgumentTransformation]
        public IEnumerable<string> TranformItemToString(Table table)
        {
            return table.Rows.Select(s => s.GetString("Item")).ToArray();
        }
    }
}