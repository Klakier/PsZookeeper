using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public abstract class CmdletTestsBase
    {
        protected PowershellHelpers powershell;
        protected ZookeeperHelpers zookeeper;

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
    }
}
