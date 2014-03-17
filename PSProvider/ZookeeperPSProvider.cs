using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Text;
using System.Text.RegularExpressions;
using log4net.Appender;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace Zookeeper.PSProvider
{
    [CmdletProvider("Zookeeeper", ProviderCapabilities.None)]
    public class ZookeeperPsProvider : NavigationCmdletProvider, IContentCmdletProvider
    {
        private static readonly Regex PathPattern = new Regex(".*");
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

            return new ZookeeperPsDriverInfo(drive)
            {
            };
        }

        protected override bool IsValidPath(string path)
        {
            return PathPattern.IsMatch(path);
        }

        protected override void SetItem(string path, object value)
        {
            Console.WriteLine("Set item");
            base.SetItem(path, value);
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            var normalizedPath = this.NormalizePath(path);
            foreach (var item in this.ZookeeperDriver.Execute(s => s.GetChildren(normalizedPath, false)))
            {
                this.WriteItemObject(item, item, true);
            }
        }

        protected override void GetItem(string path)
        {
            var stat = this.GetState(path);
            if (stat == null)
            {
                return;
            }

            var data = this.GetData(path, stat);
            var nodeInfo = new NodeInfo
            {
                Data = data,
                NumberOfChildren = stat.NumChildren,
                Version = stat.Version
            };

            WriteItemObject(nodeInfo, path, true);
        }

        private string GetData(string path, Stat stat)
        {
            var normalizedPath = this.NormalizePath(path);
            var data = this.ZookeeperDriver.Execute(z => z.GetData(normalizedPath, false, stat));
            return Encoding.Default.GetString(data);
        }

        protected override string[] ExpandPath(string path)
        {
            Console.WriteLine("ExpandPath");
            return base.ExpandPath(path);
        }

        protected override bool HasChildItems(string path)
        {
            var stat = this.GetState(path);
            return stat.NumChildren != 0;
        }

        protected override string GetChildName(string path)
        {
            var normalizedPath = this.NormalizePath(path);
            if (!normalizedPath.EndsWith(ZookeeperPsDriverInfo.WildCard))
            {
                return base.GetChildName(path);
            }

            var baseResult = base.GetChildName(path);
            var lastSeperator = normalizedPath.LastIndexOf(ZookeeperPsDriverInfo.Separator);
            lastSeperator = lastSeperator >= 0 ? lastSeperator + 1 : lastSeperator;

            var knownPart = normalizedPath.Substring(0, lastSeperator);
            var searchPart = normalizedPath.Substring(lastSeperator, normalizedPath.Length - lastSeperator);

            knownPart = knownPart.TrimEnd('/');
            knownPart = string.IsNullOrEmpty(knownPart) ? ZookeeperPsDriverInfo.Separator : knownPart;
            var items = this.ZookeeperDriver.Execute(s => s.GetChildren(knownPart, false));

            var pattern = searchPart.Replace("*", ".*");

            var result = items.FirstOrDefault(s => Regex.IsMatch(s, pattern));
            return result;
        }

        protected override string GetParentPath(string path, string root)
        {
            var result = base.GetParentPath(path, root);
            return result;
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            if (!itemTypeName.Equals("node", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Only supported item type name is 'Node'");
            }

            if (newItemValue != null && !(newItemValue is string))
            {
                throw new NotSupportedException("Only strings are supported as new item value");
            }

            var normalizedPath = this.NormalizePath(path);
            var date = newItemValue == null ? new byte[0] : Encoding.Default.GetBytes(newItemValue.ToString());
            this.ZookeeperDriver.Execute(z => z.Create(normalizedPath, date, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent));
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

        protected override bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
        {
            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }

        protected override object GetChildNamesDynamicParameters(string path)
        {
            return base.GetChildNamesDynamicParameters(path);
        }

        protected override object ItemExistsDynamicParameters(string path)
        {
            return base.ItemExistsDynamicParameters(path);
        }

        protected override object SetItemDynamicParameters(string path, object value)
        {
            return base.SetItemDynamicParameters(path, value);
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            var normalizedPath = NormalizePath(path);
            var childrens = this.ZookeeperDriver.Execute(z => z.GetChildren(normalizedPath, false));
            foreach (var children in childrens)
            {
                WriteItemObject(children, children, true);
            }
        }

        protected override void InvokeDefaultAction(string path)
        {
            base.InvokeDefaultAction(path);
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            Console.WriteLine("Intialize default drivers");
            return base.InitializeDefaultDrives();
        }

        protected override bool ItemExists(string path)
        {
            var state = GetState(path);
            return state != null;
        }

        private Stat GetState(string path)
        {
            var normalizedPath = this.NormalizePath(path);
            var state = this.ZookeeperDriver.Execute(z => z.Exists(normalizedPath, false));

            return state;
        }

        protected override string NormalizeRelativePath(string path, string basePath)
        {
            return base.NormalizeRelativePath(path, basePath);
        }

        protected override object GetItemDynamicParameters(string path)
        {
            return base.GetItemDynamicParameters(path);
        }

        private string NormalizePath(string path)
        {
            var p = path.IndexOf(':');
            if (p < 0)
            {
                return path.Replace(@"\", "/");
            }

            path = path.Remove(0, p + 1);
            path = path.TrimStart('\\');
            path = "/" + path;
            return path.Replace(@"\", "/");
        }

        public ZookeeperPsDriverInfo ZookeeperDriver
        {
            get { return (ZookeeperPsDriverInfo)this.PSDriveInfo; }
        }

        public IContentReader GetContentReader(string path)
        {
            var normalizedPath = this.NormalizePath(path);
            var stat = this.GetState(path);

            var data = this.ZookeeperDriver.Execute(z => z.GetData(normalizedPath, false, stat));

            var encoding = GetEncoding();
            if (encoding == EncodingType.Utf8)
            {
                return new Utf8ContentReader(data);
            }
            return new ZookeeperContentReader(stat, data);
        }

        private EncodingType GetEncoding()
        {
            var dynamicParametes = this.DynamicParameters as GetContentDynamicParameters;
            var encoding = dynamicParametes != null ? dynamicParametes.Encoding : EncodingType.Default;
            return encoding;
        }

        public object GetContentReaderDynamicParameters(string path)
        {
            return new GetContentDynamicParameters();
        }

        public IContentWriter GetContentWriter(string path)
        {
            var normalizedPath = this.NormalizePath(path);
            var encoding = this.GetEncoding();
            if (encoding == EncodingType.Utf8)
            {
                return new Utf8ContentWriter(this.ZookeeperDriver, normalizedPath);
            }
            return new ZookeeperContentWriter(this.ZookeeperDriver, normalizedPath);
        }

        public object GetContentWriterDynamicParameters(string path)
        {
            return new SetContentWriterDynamicParameters();
        }

        public void ClearContent(string path)
        {
            var normalizePath = this.NormalizePath(path);
            var stat = this.GetState(normalizePath);

            this.ZookeeperDriver.Execute(z => z.SetData(normalizePath, new byte[0], stat.Version));
        }

        public object ClearContentDynamicParameters(string path)
        {
            return null;
        }
    }

    public class ZookeeperContentWriter : IContentWriter
    {
        public ZookeeperContentWriter(ZookeeperPsDriverInfo connection, string normalizedPath)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IList Write(IList content)
        {
            throw new NotImplementedException();
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }

    public class Utf8ContentReader : IContentReader
    {
        private readonly byte[] _data;
        private bool _wasRead;

        public Utf8ContentReader(byte[] data)
        {
            this._data = data;
        }

        public void Dispose()
        {
        }

        public IList Read(long readCount)
        {
            if (this._wasRead)
            {
                return new ArrayList();
            }

            var value = Encoding.UTF8.GetString(this._data);

            this._wasRead = true;

            return new ArrayList() { value };
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            if (offset < 0 && (origin == SeekOrigin.Current) || origin == SeekOrigin.End)
            {
                this._wasRead = false;
            }

            if (origin == SeekOrigin.Begin && offset > 0)
            {
                this._wasRead = true;
            }
        }

        public void Close()
        {
        }
    }

    public class SetContentWriterDynamicParameters : GetContentDynamicParameters
    {
    }

    public class GetContentDynamicParameters
    {
        [Parameter]
        public EncodingType Encoding { get; set; }

    }

    public enum EncodingType
    {
        Default,
        Utf8,
    }
}
