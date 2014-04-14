using System;
using NUnit.Framework;
using Sodao.Zookeeper;
using System.Linq;

namespace Zookeeper.PSProvider.Intergration.Tests.Nunit.Helpers
{
  public class ZookeeperHelpers : IDisposable
  {
      private ZookClient zookeeper;

      public ZookeeperHelpers()
      {
          this.zookeeper= new ZookClient(string.Empty, "127.0.0.1:2181", TimeSpan.FromSeconds(10), new LogWatcher());
      }

      public bool Exist( string path )
      {
          return this.zookeeper.Exists( path, false ).Result != null;
      }

      public void CleanZookeeper()
      {
          if( this.zookeeper == null )
          {
              return;
          }
          var items = this.zookeeper.GetChildren("/", false).Result.Where(s => s != "zookeeper").ToArray();

          foreach (var item in items)
          {
              this.Remove("/" + item);
          }
      }

      private void Remove(string item)
      {
          var subItems = this.zookeeper.GetChildren2("/", false).Result;
          foreach (var subItem in this.zookeeper.GetChildren(item, false).Result)
          {
              this.Remove(item + "/" + subItem);
          }

          this.zookeeper.Delete(item, -1).Wait();
      }

      public void Dispose()
      {
          this.Dispose( true );
          GC.SuppressFinalize(this);
      }

      protected virtual void Dispose( bool dispose )
      {
          this.zookeeper.Stop();
      }
  }
}
