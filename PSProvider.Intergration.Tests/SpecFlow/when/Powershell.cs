using TechTalk.SpecFlow;

namespace Zookeeper.PSProvider.Intergration.Tests.When
{
    [Binding]
    public class Powershell
    {
        private readonly Context _context;

        public Powershell(Context context)
        {
            this._context = context;
        }

        [When(@"Powershell execute following script '(.*)'")]
        public void WhenPowershellExecuteFollowingScript(string script)
        {
            this._context.PowershellHost.AddScript(script);
            this._context.PowershellHost.Invoke();
            this._context.PowershellHost.Commands.Clear();
        }
    }
}