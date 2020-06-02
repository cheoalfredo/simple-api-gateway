using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApiGw.Extensions
{
    public static class UrlExtensions
    {
        public static string ReplaceUrlPaths(this string html, string basePath)
        {
            var newHtml = html.Replace(@"href=\""./", $"href=\"{basePath}/").Replace("src=./", $"src={basePath}/");
            return newHtml;
        }
    }
}
 