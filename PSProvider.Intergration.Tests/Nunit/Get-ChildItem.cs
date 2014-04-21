using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class GetChildItem : CmdletTestsBase
    {
        [Test]
        public void GetChildItem_should_return_all_items_in_path()
        {
            this.powershell.AddScript("(Get-ChildItem).Name");

            var result = this.powershell.Execute<string>().ToArray();

            Assert.AreEqual(new[] { "zookeeper" }, result);
        }

        [Test]
        public void Get_ChildItem_Recurse_should_return_items_recurse()
        {
            this.powershell.AddScript("New-Item -name TestItem");
            this.powershell.AddScript(@"New-Item -name SubItem -Path .\TestItem");

            this.powershell.AddScript("(Get-ChildItem -Recurse).Name");
            var result = this.powershell.Execute().ToList();

            Assert.IsNotEmpty(result);

            Assert.Contains("TestItem", result);
            Assert.Contains("SubItem", result);
        }

        [Test]
        public void Get_ChildItem_should_get_item_when_wildcard_is_used()
        {
            this.powershell.AddScript("New-Item -name Test");
            this.powershell.AddScript("(ls Tes*) -ne $null");

            var result = this.powershell.Execute<bool>().First();

            Assert.IsTrue(result);
        }

        [Test]
        public void Get_ChildItem_should_return_elements_recurse_if_Recurse_flag_is_passed()
        {
            var expectedValue = new []
            {
                "SubTest",
                "SubSubTest2",
                "SubSubTest1"
            };

            this.powershell.AddScript("New-Item -name Test");
            this.powershell.AddScript(@"New-Item -name SubTest -Path .\Test\");
            this.powershell.AddScript(@"New-Item -name SubSubTest1 -Path .\Test\SubTest");
            this.powershell.AddScript(@"New-Item -name SubSubTest2 -Path .\Test\SubTest");
            this.powershell.AddScript(@"(Get-ChildItem -Recurse -Path Tes*).Name");

            var result = this.powershell.Execute<string>().ToArray();

            CollectionAssert.AreEquivalent(expectedValue, result);
        }

        [Test]
        public void Get_ChildItem_with_flag_Recurse_without_path_should_retrun_elements_Recurse()
        {
            var expectedValues = new []
            {
                "/" ,
                "Test" ,
                "SubTest" ,
                "zookeeper" ,
                "quota"
            };

            this.powershell.AddScript(@"New-Item -name Test");
            this.powershell.AddScript(@"New-Item -name SubTest -Path .\Test\");
            this.powershell.AddScript(@"(Get-ChildItem -Recurse).Name");

            var result = this.powershell.Execute<string>().ToArray();

            CollectionAssert.AreEquivalent(expectedValues, result);

        }

        [Test]
        public void Get_ChildItem_should_set_PsPath_to_rooted_full_path()
        {
            this.powershell.AddScript(@"New-Item -name Test");
            this.powershell.AddScript(@"(Get-ChildItem Tes*).PsPath");

            var result = this.powershell.Execute<string>().First();

            Assert.IsTrue(result.EndsWith(@"Zookeeper:\Test"));
        }
    }
}
