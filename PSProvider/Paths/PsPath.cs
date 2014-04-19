using System;

namespace Zookeeper.PSProvider.Paths
{
    public class PsPath
    {
        public const string Separator = @"\";

        public static string FromZookeeperPath( ZookeeperPsDriverInfo driverInfo, string path ) 
        {
            var windowsPath = path.Replace(ZookeeperPath.Separator, PsPath.Separator );
            if( !IsRooted( windowsPath ) )
            {
                return EnforceIsRooted( driverInfo, windowsPath );
            }

            return windowsPath;
        }

        public static string Join( string path, string children )
        {
            if( path.EndsWith(Separator) || children.StartsWith(Separator))
            {
                return path + children;
            }

            return path + Separator + children;
        }

        private static bool IsRooted( string path )
        {
            return path.IndexOf(":") != -1;
        }

        private static string EnforceIsRooted( ZookeeperPsDriverInfo driverInfo, string path )
        {
            if( path.StartsWith( Separator ) )
            {
                return driverInfo.Name + ":" + path;
            }

            return driverInfo.Name + ":" + driverInfo.Root + path;
        }
    }
}
