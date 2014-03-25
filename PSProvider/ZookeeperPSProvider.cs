using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text.RegularExpressions;
using Zookeeper.PSProvider.Serializer;

namespace Zookeeper.PSProvider
{
    [CmdletProvider("Zookeeeper", ProviderCapabilities.ExpandWildcards)]
    public class ZookeeperPsProvider : NavigationCmdletProvider, IContentCmdletProvider
    {
        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            // check if drive object is null
            if (drive == null)
            {
                WriteError(new ErrorRecord(
                    new ArgumentNullException("drive"),
                    "NullDrive",
                    ErrorCategory.InvalidArgument,
                    null)
                );

                return null;
            }

            // check if drive root is not null or empty
            // and if its an existing file
            if (String.IsNullOrEmpty(drive.Root))
            {
                WriteError(new ErrorRecord(
                    new ArgumentException("drive.Root"),
                    "NoRoot",
                    ErrorCategory.InvalidArgument,
                    drive)
                );

                return null;
            }

            var configuration = (this.DynamicParameters as Configuration) ?? new Configuration();

            return new ZookeeperPsDriverInfo(drive, ZookeeperFactory.Create(configuration));
        }

        protected override object NewDriveDynamicParameters()
        {
            return new Configuration();
        }

        protected override bool IsValidPath(string path)
        {
            return ZookeeperPath.IsValid(path);
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            foreach (var subItems in this.ZookeeperDriver.Zookeeper.GetChildren(path))
            {
                this.WriteItemObject(subItems, subItems, true);
            }
        }

        protected override void GetItem(string path)
        {
            var item = this.ZookeeperDriver.Zookeeper.GetItem(path);
            WriteItemObject(item, path, true);
        }

        protected override string[] ExpandPath(string path)
        {
            throw new NotSupportedException("ExpandPath");
        }

        protected override bool HasChildItems(string path)
        {
            var stat = this.ZookeeperDriver.Zookeeper.GetStat(path);
            return stat.NumChildren != 0;
        }

        protected override string GetChildName(string path)
        {
            var tokens = ZookeeperPath.Tokenize(path);
            if (!tokens.HasWildCard)
            {
                return base.GetChildName(path);
            }

            var regex = new Regex(tokens.WildCardPattern);

            var result = this.ZookeeperDriver.Zookeeper.GetChildren(tokens.KnwonPath).FirstOrDefault(regex.IsMatch);
            return result;
        }

        protected override bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
        {
            Console.WriteLine("Convert path");
            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            var parameters = (this.DynamicParameters as NewItemParamters) ?? new NewItemParamters();
            if (!string.IsNullOrWhiteSpace(itemTypeName))
            {
                this.WriteWarning("itemTypeName is ignored");
            }

            this.ZookeeperDriver.Zookeeper.CreateItem(
                path,
                this.GetSerializer(parameters.Encoding).Serialize(newItemValue),
                parameters.CreateMode);
        }

        private ISerializer GetSerializer(EncodingType encoding)
        {
            switch (encoding)
            {
                case EncodingType.Raw:
                    return new RawSerializer();
                case EncodingType.Utf8:
                default:
                    return new Utf8Serializer();
            }
        }

        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            return new NewItemParamters();
        }

        protected override bool IsItemContainer(string path)
        {
            return this.ItemExists(path);
        }

        protected override string MakePath(string parent, string child)
        {
            var result = base.MakePath(parent, child);
            return result;
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            var childrens = recurse
                ? this.ZookeeperDriver.Zookeeper.GetChildrenRecurse(path)
                : this.ZookeeperDriver.Zookeeper.GetChildren(path);

            foreach (var children in childrens)
            {
                WriteItemObject(children, children, true);
            }
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            Console.WriteLine("Intialize default drivers");
            return base.InitializeDefaultDrives();
        }

        protected override bool ItemExists(string path)
        {
            return this.ZookeeperDriver.Zookeeper.PathExist(path);
        }

        public ZookeeperPsDriverInfo ZookeeperDriver
        {
            get { return (ZookeeperPsDriverInfo)this.PSDriveInfo; }
        }

        public IContentReader GetContentReader(string path)
        {
            var parametes = (this.DynamicParameters as GetContentDynamicParameters) ?? new GetContentDynamicParameters();
            var data = this.ZookeeperDriver.Zookeeper.GetData(path);
            var contentReader = this.GetReader(parametes.Encoding, data.Data);
            return contentReader;
        }

        private IContentReader GetReader(EncodingType encoding, byte[] data)
        {
            switch (encoding)
            {
                case EncodingType.Raw:
                    return new RawContentReader(data);
                case EncodingType.Utf8:
                default:
                    return new Utf8ContentReader(data);
            }
        }

        public object GetContentReaderDynamicParameters(string path)
        {
            return new GetContentDynamicParameters();
        }

        public IContentWriter GetContentWriter(string path)
        {
            var paramets = (this.DynamicParameters as GetContentDynamicParameters) ?? new GetContentDynamicParameters();
            return this.GetWriter(paramets.Encoding, path, paramets.Version);
        }

        private IContentWriter GetWriter(EncodingType encoding, string path, int version)
        {
            switch (encoding)
            {
                case EncodingType.Raw:
                    return new ZookeeperContentWriter(data => this.ZookeeperDriver.Zookeeper.SetData(path, data, version));
                case EncodingType.Utf8:
                default:
                    return new Utf8ContentWriter(data => this.ZookeeperDriver.Zookeeper.SetData(path, data, version));
            }
        }

        public object GetContentWriterDynamicParameters(string path)
        {
            return new SetContentWriterDynamicParameters();
        }

        public void ClearContent(string path)
        {
            this.ZookeeperDriver.Zookeeper.SetData(path, new byte[0], Zookeeper.AnyVersion);
        }

        public object ClearContentDynamicParameters(string path)
        {
            return null;
        }
    }
}
