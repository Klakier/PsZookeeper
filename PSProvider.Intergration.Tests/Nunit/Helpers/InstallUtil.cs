using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers
{
    public static class InstallUtil
    {
        private static readonly string InstallUtilPath =
            Environment.GetFolderPath(Environment.SpecialFolder.Windows)
            + @"\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe";

        public static void Install(string assemblyName)
        {
            RunInstallUtill(assemblyName + ".dll");
        }

        public static void Uninstall(string assemblyName)
        {
            RunInstallUtill("/u " + assemblyName + ".dll");
        }

        private static void RunInstallUtill(string arguments)
        {
            var processInfo = new ProcessStartInfo(InstallUtilPath, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            var process = Process.Start(processInfo);

            Assert.NotNull(process);

            Console.WriteLine(process.StandardOutput.ReadToEnd());

            Assert.True(process.WaitForExit((int)TimeSpan.FromSeconds(10).TotalMilliseconds));


            Assert.AreEqual(0, process.ExitCode);
        }
    }
}
