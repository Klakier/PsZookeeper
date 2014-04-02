using System;
using NUnit.Framework;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;
using Zookeeper.PSProvider.Intergration.Tests.Utils;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class RemoveItem
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
        public void When_invoke_Remove_Item_item_should_be_removed()
        {
            this.powershell.AddScript( "New-Item -name TestItem" );

            this.powershell.AddScript( "Remove-Item -Path TestItem" );

            this.powershell.Execute();

            Assert.IsFalse( this.zookeeper.Exist( "/TestItem" ) );
        }

        [Test]
        public void Remove_Item_should_fail_if_item_has_children_and_flag_force_was_not_set()
        {
            this.powershell.AddScript( "New-Item -name TestItem" );
            this.powershell.AddScript( @"New-Item -name SubItem -Path .\TestItem" );

            this.powershell.AddScript( "Remove-Item -Path TestItem" );

            this.powershell.Execute();

            Assert.IsTrue( this.zookeeper.Exist( "/TestItem" ) );
        }

        [Test]
        public void Remove_Item_should_remove_item_with_all_sub_items_if_Recurse_flag_is_set()
        {
            this.powershell.AddScript( "New-Item -name TestItem" );
            this.powershell.AddScript( @"New-Item -name SubItem -Path .\TestItem" );
            
            this.powershell.AddScript( "Remove-Item -Path TestItem -Recurse" );

            this.powershell.Execute();

            Assert.IsFalse( this.zookeeper.Exist( "/TestItem" ) );
        }
    }
}

