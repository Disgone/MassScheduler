using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace MassScheduler.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString UploadedImage(this HtmlHelper helper, string fileName, object htmlAttributes = null)
        {
            var uploadDirectory = WebConfigurationManager.AppSettings["UploadDirectory"];
            var uploadPath = Path.Combine(uploadDirectory, fileName);
            var imageUrl = VirtualPathUtility.ToAbsolute(uploadPath);

            var tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes.Add("src", imageUrl);

            var attributes = new RouteValueDictionary(htmlAttributes);
            tagBuilder.MergeAttributes(attributes);

            return new HtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}