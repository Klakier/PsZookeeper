using System;
using NUnit.Framework;
using System.Linq;
using Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit
{
    [TestFixture(Category="Integration_tests")]
    public class SetLocation : CmdletTestsBase
    {
        [Test]
        public void SetLocation_should_change_current_dictionary()
        {
            this.powershell.AddScript( "Set-Location zookeeper" );
            this.powershell.AddScript( "$pwd.Path" );

            var result = this.powershell.Execute<string>().ToArray();

            CollectionAssert.AreEquivalent( new [] { @"Zookeeper:\zookeeper" }, result );
        }
    }
}
