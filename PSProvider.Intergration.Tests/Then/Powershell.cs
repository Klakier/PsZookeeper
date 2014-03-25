using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Zookeeper.PSProvider.Intergration.Tests.Then
{
    [Binding]
    public class Powershell
    {
        private readonly Context _context;

        public Powershell(Context context)
        {
            this._context = context;
        }

        [Then(@"Executing script '(.*)' should return true")]
        public void ThenExecutingScriptShouldReturnTrue(string script)
        {
            this._context.PowershellHost.AddScript(script);
            var result1 = this._context.PowershellHost.Invoke<bool>().ToArray();

            Assert.IsNotEmpty(result1);
            Assert.IsTrue(result1.First());
        }

        [Then(@"Executing script '(.*)' should not generate errors")]
        public void ThenExecutingScriptShouldNotGenerateErrors(string script)
        {
            this._context.PowershellHost.AddScript(script);
            this._context.PowershellHost.Invoke();
            this._context.PowershellHost.Commands.Clear();
        }

        [Then(@"Executing script '(.*)' should return following items")]
        public void ThenExecutingScriptShouldReturnFollowingItems(string script, IEnumerable<string> result)
        {
            this._context.PowershellHost.AddScript(script);
            var psResult = this._context.PowershellHost.Invoke<object>();

            this._context.PowershellHost.Commands.Clear();

            Assert.AreEqual(result, psResult);
        }
    }
}