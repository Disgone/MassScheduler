using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using MarkdownSharp;

namespace MassScheduler.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString UploadedImage(this HtmlHelper helper, string fileName, object htmlAttributes = null)
        {
            if(string.IsNullOrEmpty(fileName))
                return new MvcHtmlString(string.Empty);

            var uploadDirectory = WebConfigurationManager.AppSettings["UploadDirectory"];
            var uploadPath = Path.Combine(uploadDirectory, fileName);
            var imageUrl = VirtualPathUtility.ToAbsolute(uploadPath);

            var tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes.Add("src", imageUrl);

            SetHtmlAttributes(htmlAttributes, tagBuilder);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        private static readonly Markdown MarkdownTransformer = new Markdown();

        public static IHtmlString Markdown(this HtmlHelper helper, string text)
        {
            var html = MarkdownTransformer.Transform(text);
            return new MvcHtmlString(html);
        }

        public static IHtmlString MailTo(this HtmlHelper helper, string email, string text, object htmlAttributes = null)
        {
            var tagBuilder = new TagBuilder("a");
            tagBuilder.SetInnerText(text);
            tagBuilder.Attributes.Add("href", string.Format("mailto:{0}", email));

            SetHtmlAttributes(htmlAttributes, tagBuilder);

            return new MvcHtmlString(tagBuilder.ToString());
        }

        private static void SetHtmlAttributes(object htmlAttributes, TagBuilder tagBuilder)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            tagBuilder.MergeAttributes(attributes);
        }
    }
}