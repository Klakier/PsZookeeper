using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text.RegularExpressions;
using Zookeeper.PSProvider.Serializer;
using Zookeeper.PSProvider.Paths;
using Zookeeper.PSProvider.ItemsRepresntation;

namespace Zookeeper.PSProvider
{
    [CmdletProvider("Zookeeeper", ProviderCapabilities.Filter)]
    [OutputType( new Type[] {typeof(NodeInfo)}, ProviderCmdlet = "Get-Item")]
    [OutputType( new Type[] {typeof(ShortNodeRepresentation)}, ProviderCmdlet = "Get-Items")]
    public class ZookeeperPsProvider : 
        NavigationCmdletProvider,
        IContentCmdletProvider
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

#region Container cmdltet

        protected override void RemoveItem(string path, bool recurse)
        {
            if (!recurse && this.HasChildItems(path))
            {
                throw new InvalidOperationException("Can not remove node because is not empty");
            }

            this.ZookeeperDriver.Zookeeper.Remove(path, recurse);
        }

#endregion

        protected override bool IsValidPath(string path)
        {
            var result = ZookeeperPath.IsValid(path);

            return result;
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            foreach (var subItems in this.ZookeeperDriver.Zookeeper.GetChildren(path))
            {
                this.WriteItemObject(subItems, path + PsPath.Separator + subItems, true);
            }
        }

        protected override void GetItem(string path)
        {
            var exit = this.ZookeeperDriver.Zookeeper.PathExist(path);
            if (!exit)
            {
                return;
            }

            var data = this.ZookeeperDriver.Zookeeper.GetItem(path);
            var item = new LongNodeRepresentation(data, this.ZookeeperDriver, path); 

            this.WriteItemObject(item, path, true);
        }

        private void WriteShortNode(string fullPath)
        {
            var name = ZookeeperPath.GetItemName(fullPath);

            var item = new ShortNodeRepresentation(
                    name,
                    PsPath.FromZookeeperPath(this.ZookeeperDriver, fullPath));

            this.WriteItemObject(item, fullPath, true);
        }

        protected override bool HasChildItems(string path)
        {
            var stat = this.ZookeeperDriver.Zookeeper.GetStat(path);
            var result = stat != null && stat.NumChildren != 0;

            return result;
        }

        protected override string GetChildName(string path)
        {
            path = path.Replace('/', '\\');
            path = path.TrimEnd('\\');

            int num = path.LastIndexOf('\\');
            if (num == -1)
            {
                return this.EnsureDriveIsRooted(path);
            }

            return path.Substring(num + 1);

        }

        private string EnsureDriveIsRooted(string path)
        {
            int num = path.IndexOf(':');
            if (num != -1 && num + 1 == path.Length)
            {
                path += '\\';
            }

            return path;
        }

        protected override bool ConvertPath(
                string path,
                string filter,
                ref string updatedPath,
                ref string updatedFilter)
        {
            return false;
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

        protected override object NewItemDynamicParameters(
                string path,
                string itemTypeName,
                object newItemValue)
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
            if (recurse)
            {
                this.GetChildrenRecurse(path, this.WriteShortNode);
            } else
            {
                foreach (var children in this.ZookeeperDriver.Zookeeper.GetChildren(path))
                {
                    this.WriteShortNode(PsPath.Join(path, children));
                }
            }
        }

        private void GetChildrenRecurse(string path, Action<string> itemAction)
        {
            var normalizePath = ZookeeperPath.Normalize(path);
            itemAction(normalizePath);
            foreach (var item in this.ZookeeperDriver.Zookeeper.GetChildren(path))
            {
                var fullPath = ZookeeperPath.Join(normalizePath, item);
                this.GetChildrenRecurse(fullPath, itemAction);
            }
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            return base.InitializeDefaultDrives();
        }

        protected override bool ItemExists(string path)
        {
            var result = this.ZookeeperDriver.Zookeeper.PathExist(path);

            return result;
        }

        public ZookeeperPsDriverInfo ZookeeperDriver
        {
            get { return (ZookeeperPsDriverInfo)this.PSDriveInfo; }
        }

        public IContentReader GetContentReader(string path)
        {
            var parametes = (this.DynamicParameters as GetContentDynamicParameters) 
                ?? new GetContentDynamicParameters();
            var data = this.ZookeeperDriver.Zookeeper.GetItem(path);
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
