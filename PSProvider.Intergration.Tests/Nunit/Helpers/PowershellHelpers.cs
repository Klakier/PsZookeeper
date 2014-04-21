
using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers
{
    public class PowershellHelpers
    {
        private PowerShell powershell;

        public PowershellHelpers()
        {
            this.powershell = PowerShell.Create();

            this.powershell.AddScript("$DebugPreference = 'Continue'");
        }

        public void AddScript(string script)
        {
            this.powershell.AddScript(script);
        }

        public IEnumerable<PSObject> Execute()
        {
            return this.Execute<PSObject>();
        }

        public bool HadErrors
        {
            get { return this.powershell.HadErrors; }
        }

        public IEnumerable<T> Execute<T>()
        {
            try
            {
                var results = this.powershell.Invoke<T>().ToArray();

                return results;
            } 
            finally
            {
                this.PrintStreams(this.powershell.Streams);
                powershell.Commands.Clear();
            }
        }

        private void PrintStreams(PSDataStreams streams)
        {
            this.PrintErrors(streams.Error);
            this.PrintInformationalRecord("Waring", streams.Warning);
            this.PrintInformationalRecord("Verbose", streams.Verbose);
            this.PrintInformationalRecord("Debug", streams.Debug);
        }

        private void PrintErrors(IEnumerable<ErrorRecord> records)
        {
            Console.WriteLine( "ERROR: {0}", records.Count() );
            foreach (ErrorRecord record in records)
            {
                Console.WriteLine(
                  "ERROR: {0}:{1} - '{2}'" 
                    + System.Environment.NewLine 
                    + "Exception: {3}",
                  record.InvocationInfo.ScriptName,
                  record.InvocationInfo.Line, 
                  record.ErrorDetails,
                  record.Exception);
            }
        }

        private void PrintInformationalRecord(string prefix, IEnumerable<InformationalRecord> records)
        {
            Console.WriteLine("{0}: {1}", prefix.ToUpper(), records.Count() );
            foreach (InformationalRecord record in records)
            {
                Console.WriteLine(
                    "{0}: {1}:{2} - '{3}'",
                    prefix,
                    record.InvocationInfo.ScriptName,
                    record.InvocationInfo.Line,
                    record.Message);
            }
        }
    }
}
