using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Utility
{
    public class DocumentHelper
    {
        public static async Task<string> HandleImages(IFormFile ImageFile, string OldImageFileName = null)
        {
            if (ImageFile == null || ImageFile.Length == 0) return null;

            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
            var filePath = Path.Combine(imagesFolder, fileName);

            // Delete old image
            if (!string.IsNullOrEmpty(OldImageFileName))
            {

                var oldImagePath = Path.Combine(imagesFolder, OldImageFileName);
                if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
            }

            using var stream = new FileStream(filePath, FileMode.Create);
            await ImageFile.CopyToAsync(stream);

            return fileName;

        }
    }
}
