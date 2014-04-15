using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class NewItem : CmdletTestsBase
    {
        [Test]
        public void NewItem_should_create_new_item()
        {
            this.powershell.AddScript( "New-Item -name Test -ItemType Node -Value 'Test'" );
            this.powershell.AddScript( "(Get-ChildItem).Name" );

            var result = this.powershell.Execute<string>();

            CollectionAssert.Contains( result, "Test" );
        }

        [Test]
        public void NewItem_should_be_able_create_new_items_in_sub_folders()
        {
            this.powershell.AddScript("New-Item -name Test");
            this.powershell.AddScript(@"New-Item -name SubTest -Path .\Test");
            this.powershell.AddScript(@"(Get-ChildItem .\Test).Name");

            var result = this.powershell.Execute<string>().First();

            Assert.AreEqual("SubTest", result);
        }
    }
}
