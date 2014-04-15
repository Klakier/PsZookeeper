using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class SetContent : CmdletTestsBase
    {
        [Test]
        public void Set_Content_with_encoding_should_change_data()
        {
            this.powershell.AddScript("New-Item -name Test -ItemType Node -Value TestValue");
            this.powershell.AddScript("Set-Content -Path Test -Encoding UTF8 -Value TestValue2");
            this.powershell.AddScript("Get-Content -Path Test -Encoding UTF8");

            var result = this.powershell.Execute<string>().First();

            Assert.AreEqual("TestValue2", result );
        }
    }
}
