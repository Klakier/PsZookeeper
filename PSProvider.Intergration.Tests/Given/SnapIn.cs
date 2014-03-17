using System;
using System.Diagnostics;
using System.Security.Permissions;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Zookeeper.PSProvider.Intergration.Tests.Utils;

namespace Zookeeper.PSProvider.Intergration.Tests.Given
{
    [Binding]
    public class SnapIn
    {
        private readonly Context _context;

        public SnapIn(Context context)
        {
            this._context = context;
        }

        [Given(@"I have registered SnapIn from assembly '(.*)'")]
        public void GivenIHaveRegisteredSnapInFromAssembly(string assemblyName)
        {
            this._context.RegisterdAssemblies.Add(assemblyName);
            InstallUtil.Install(assemblyName);
        }
    }
}