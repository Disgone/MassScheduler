using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Configuration;

namespace MassScheduler.Helpers
{
    public class UploadHelper
    {
        public static string HandleUploadedImage(HttpPostedFileBase photo, int maxWidth = 200, int maxHeight = 200)
        {
            var uploadDirectory = WebConfigurationManager.AppSettings["UploadDirectory"];
            var uploadPath = HttpContext.Current.Server.MapPath(uploadDirectory);

            CreateDirectoryIfNotExists(uploadPath);

            if (photo != null && photo.ContentLength > 0)
            {
                var hashedName = photo.GetHashCode();
                var path = Path.Combine(uploadPath, String.Format("{0}.jpg", hashedName));
                photo.SaveAs(path);

                var temp = Image.FromFile(path);
                var final = ImageHelper.ScaleImage(temp, maxWidth, maxHeight);
                temp.Dispose();
                
                final.Save(path, ImageFormat.Jpeg);
                final.Dispose();

                return Path.GetFileName(path);
            }

            return null;
        }

        protected static void CreateDirectoryIfNotExists(string path)
        {
            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
        }
    }
}