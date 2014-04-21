using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class GetContent : CmdletTestsBase
    {
        [Test]
        public void GetContent_should_return_data_using_given_encoding()
        {
            this.powershell.AddScript("New-Item -name Test -ItemType Node -Value TestValue");
            this.powershell.AddScript("Get-Content -Path Test -Encoding UTF8");

            var result = this.powershell.Execute<string>().First();

            Assert.AreEqual("TestValue", result );
        }

        [Test]
        public void GetContent_should_be_able_get_content_from_items_in_sub_folders()
        {
            this.powershell.AddScript("New-Item -name Test -ItemType Node -Value TestValue");
            this.powershell.AddScript("cd Test");
            this.powershell.AddScript("New-Item -name SubTest -ItemType Node -Value SubTestValue");
            this.powershell.AddScript("Get-Content SubTest -Encoding UTF8");

            var result = this.powershell.Execute<string>().First();

            Assert.AreEqual("SubTestValue", result );
        }

        [Test]
        public void GetContent_should_not_generate_any_errors_even_when_content_is_empty()
        {
            this.powershell.AddScript("New-Item -name Test");

            this.powershell.AddScript("Get-content Test");
            this.powershell.Execute();

            Assert.IsFalse( this.powershell.HadErrors );
        }
    }
}
