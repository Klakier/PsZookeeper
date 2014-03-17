using TechTalk.SpecFlow;

namespace Zookeeper.PSProvider.Intergration.Tests.Hooks
{
    [Binding]
    public class Zookeeper
    {
        private readonly Context _context;

        public Zookeeper(Context context)
        {
            this._context = context;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (this._context.ZookeeperClient != null)
            {
                this._context.ZookeeperClient.Dispose();
            }
        }
    }
}