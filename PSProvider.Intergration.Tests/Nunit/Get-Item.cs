using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class GetItem
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

        [TearDown]
        public void TearDown()
        {
            this.zookeeper.Dispose();
        }

        [Test]
        public void Get_Item_should_return_appropiate_item()
        {
            this.powershell.AddScript("New-Item -name Test -ItemType Node -Value TestValue");
            this.powershell.AddScript("(Get-Item -Path Test) -ne $null");

            var result = this.powershell.Execute<bool>().First();

            Assert.IsTrue( result );
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
    }
}
