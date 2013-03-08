using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace MassScheduler.Helpers
{
    public static class UrlHelper
    {
        public static string ResolveTextToUrl(string text)
        {
            return Regex.Replace(Regex.Replace(text, "[^\\w]", "-"), "[-]{2,}", "-").ToLower();
        }

        public static string ResolveTextToUrl(this HtmlHelper helper, string text)
        {
            return ResolveTextToUrl(text);
        }
    }
}