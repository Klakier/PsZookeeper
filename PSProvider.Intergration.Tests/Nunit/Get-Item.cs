using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class GetItem : CmdletTestsBase
    {
        [Test]
        public void Get_Item_should_return_appropiate_item()
        {
            this.powershell.AddScript("New-Item -name Test -ItemType Node -Value TestValue");
            this.powershell.AddScript("(Get-Item -Path Test) -ne $null");

            var result = this.powershell.Execute<bool>().First();

            Assert.IsTrue(result);
        }

        [Test]
        public void Get_Item_should_return_object_that_contains_additional_properties()
        {
            this.powershell.AddScript("new-item -Name SubItem -Value 'example value'");

            this.powershell.AddScript(
                "Get-Item .\\SubItem "
                + "| Get-Member "
                + "| ? {$_.MemberType -eq [System.Management.Automation.PSMemberTypes]::Property } "
                + "| select -ExpandProperty 'Name'");

            var result = this.powershell.Execute<string>().ToList();

            Assert.Contains("Name", result);
            Assert.Contains("FullName", result);
            Assert.Contains("Version", result);
            Assert.Contains("NumberOfChildren", result);
            Assert.Contains("Data", result);
        }

        [Test]
        public void Get_Item_should_return_item_where_FullName_is_equal_to_rooted_full_path()
        {
            this.powershell.AddScript("new-item -Name SubItem");
            this.powershell.AddScript(@"(Get-Item .\SubItem).FullName");

            var result = this.powershell.Execute<string>().First();

            Assert.AreEqual(@"Zookeeper:\SubItem", result);
        }

        [Test]
        public void Get_Item_should_return_item_where_PsPath_ends_with_to_rooted_full_path()
        {
            this.powershell.AddScript("new-item -Name SubItem");
            this.powershell.AddScript(@"(Get-Item .\SubItem).PsPath");

            var result = this.powershell.Execute<string>().First();

            Console.WriteLine("Result: {0}", result );

            Assert.IsTrue(result.EndsWith(@"Zookeeper:\SubItem"));
        }
    }
}
