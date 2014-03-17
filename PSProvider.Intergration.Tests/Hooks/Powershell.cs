using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using TechTalk.SpecFlow;

namespace Zookeeper.PSProvider.Intergration.Tests.Hooks
{
    [Binding]
    public class Powershell
    {
        private readonly Context _context;

        public Powershell(Context context)
        {
            this._context = context;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Write("Debug", this._context.PowershellHost.Streams.Debug);
            Write("Verbose", this._context.PowershellHost.Streams.Verbose);
            Write("Warning", this._context.PowershellHost.Streams.Warning);
            Write("Warning", this._context.PowershellHost.Streams.Warning);
            Write("Progress", this._context.PowershellHost.Streams.Progress);
            WriteError("Error", this._context.PowershellHost.Streams.Error);
            this._context.PowershellHost.Dispose();
        }

        private void Write(string progress, IEnumerable<ProgressRecord> psDataCollection)
        {
            foreach (var progressRecord in psDataCollection)
            {
                Console.WriteLine(progress + progressRecord.StatusDescription);
                Console.WriteLine(progress + progressRecord );
            }
        }

        private void Write(string prefix, IEnumerable<InformationalRecord> psDataCollection)
        {
            foreach (var informationalRecord in psDataCollection)
            {
                Console.WriteLine("{0}> {1}", prefix, informationalRecord.Message);
            }
        }

        private void WriteError(string prefix, IEnumerable<ErrorRecord> psDataCollection)
        {
            foreach (var informationalRecord in psDataCollection)
            {
                Console.WriteLine("{0}> {1}", prefix, informationalRecord.Exception);
            }
        }
    }
}
