using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Zookeeper.PSProvider.Intergration.Tests.Given
{
    [Binding]
    public class Powershell
    {
        private readonly Context _context;

        public Powershell(Context context)
        {
            this._context = context;
        }

        [Given(@"I have Powershell host")]
        public void GivenIHavePowershellHostWithSnapIn()
        {
            this._context.PowershellHost = PowerShell.Create();
        }

        [Given(@"Powershell add following script '(.*)'")]
        public void GivenPowershellExecutingFollowingScript(string script)
        {
            this._context.PowershellHost.AddScript(script);
        }

        [Given(@"Powershell execute scheduled commands")]
        public void GivenPowershellExecuteScheduledCommands()
        {
            this._context.PowershellHost.Invoke();
            this._context.PowershellHost.Commands.Clear();
        }
    }
}
