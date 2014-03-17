using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Zookeeper.PSProvider;

namespace PSProvider
{
    [RunInstaller(true)]
    public class ZookeeperSnapIn : CustomPSSnapIn
    {
        /// <summary>
        /// Specify the name of the custom PowerShell snap-in. 
        /// </summary>
        public override string Name
        {
            get { return "ZookeeperPSSnap"; }
        }

        public override string Vendor
        {
            get { return "Maniek"; }
        }

        public override string VendorResource
        {
            get { return "ZookeeperSnapIn,Maniek"; }
        }

        public override string Description
        {
            get { return "TODO"; }
        }

        public override string DescriptionResource
        {
            get { return "TODO"; }
        }

        private Collection<CmdletConfigurationEntry> _cmdlets;

        public override Collection<CmdletConfigurationEntry> Cmdlets
        {
            get
            {
                if (this._cmdlets == null)
                {
                    this._cmdlets = new Collection<CmdletConfigurationEntry>();
                }

                return this._cmdlets;
            }
        }

        private Collection<ProviderConfigurationEntry> _providers;

        public override Collection<ProviderConfigurationEntry> Providers
        {
            get
            {
                if (_providers == null)
                {
                    _providers = new Collection<ProviderConfigurationEntry>();
                    this._providers.Add(new ProviderConfigurationEntry("Zookeeeper", typeof(ZookeeperPsProvider), "Zookeeeper"));
                }

                return _providers;
            }
        }

        private Collection<TypeConfigurationEntry> _types;

        public override Collection<TypeConfigurationEntry> Types
        {
            get
            {
                if (_types == null)
                {
                    _types = new Collection<TypeConfigurationEntry>();
                }

                return _types;
            }
        }

        private Collection<FormatConfigurationEntry> _formats;

        public override Collection<FormatConfigurationEntry> Formats
        {
            get
            {
                if (_formats == null)
                {
                    _formats = new Collection<FormatConfigurationEntry>();
                }

                return _formats;
            }
        }
    }
}
