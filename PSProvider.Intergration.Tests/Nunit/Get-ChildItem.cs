using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;
using Zookeeper.PSProvider.Intergration.Tests.Utils;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class GetChildItem
    {
        PowershellHelpers powershell;
        ZookeeperHelpers zookeeper;

        [SetUp]
        public void SetUp()
        {
            InstallUtil.Install("Zookeeper.PSProvider");
            this.zookeeper = new ZookeeperHelpers();
            this.zookeeper.CleanZookeeper();

            this.powershell = new PowershellHelpers();
            this.powershell.AddScript("Add-PSSnapin ZookeeperPSSnap");
            this.powershell.AddScript("New-PSDrive -Name Zookeeper -PSProvider Zookeeeper -Root /");
            this.powershell.AddScript("cd Zookeeper:");
        }

        [Test]
        public void Get_ChildItem_Recurse_should_return_items_recourse()
        {
            this.powershell.AddScript( "New-Item -name TestItem" );
            this.powershell.AddScript( @"New-Item -name SubItem -Path .\TestItem" );

            this.powershell.AddScript("(Get-ChildItem -Recurse).Name");
            var result = this.powershell.Execute().ToList();

            Assert.IsNotEmpty( result );

            Assert.Contains("TestItem", result );
            Assert.Contains("SubItem", result );
        }
    }
}
