using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Zookeeper.PSProvider.Intergration.Tests.Utils;

namespace Zookeeper.PSProvider.Intergration.Tests.Hooks
{
    [Binding]
    public class SnapIn
    {
        private readonly Context _context;

        public SnapIn(Context context)
        {
            this._context = context;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            foreach (var registerdAssembly in this._context.RegisterdAssemblies)
            {
                //InstallUtil.Uninstall(registerdAssembly);
            }
        }
    }
}
