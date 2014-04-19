using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Zookeeper.PSProvider.Paths
{
    public class ZookeeperPath
    {
        public const string Root = "/";
        public const string WildCard = "*";
        public const string Separator = "/";
        public const char CharSeparator = '/';
        public const string SystemFolder = "zookeeper";

        public static string Normalize(string path)
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

        public static ZookeeperPathTokens Tokenize(string path)
        {
            var normalizedPath = Normalize(path);
            if (!normalizedPath.EndsWith(WildCard))
            {
                return new ZookeeperPathTokens(false, string.Empty, normalizedPath);
            }

            var lastSeperator = normalizedPath.LastIndexOf(Separator, StringComparison.Ordinal);
            lastSeperator = lastSeperator >= 0 ? lastSeperator + 1 : lastSeperator;

            var knownPart = normalizedPath.Substring(0, lastSeperator);
            var searchPart = normalizedPath.Substring(lastSeperator, normalizedPath.Length - lastSeperator);

            knownPart = knownPart.TrimEnd('/');
            knownPart = string.IsNullOrEmpty(knownPart) ? Separator : knownPart;

            var pattern = searchPart.Replace("*", ".*");

            return new ZookeeperPathTokens(true, pattern, knownPart);
        }

        public static bool IsValid(string path)
        {
            return true;
        }

        public static string GetItemName(string path)
        {
            path = Normalize(path);
            if (path == string.Empty)
            {
                return string.Empty;
            }

            if (path == Separator)
            {
                return Separator;
            }

            path = path.TrimEnd(CharSeparator);

            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            var lastSeparator = path.LastIndexOf(Separator, System.StringComparison.Ordinal);
            if (lastSeparator == -1)
            {
                return path;
            }

            return path.Substring(lastSeparator + 1);
        }

        public static string Join(string path, string item)
        {
            path = Normalize(path);
            var fullPath = path + Separator + item;

            const string doubleSeparator = Separator + Separator;
            return fullPath.Replace(doubleSeparator, Separator);
        }
    }
}
