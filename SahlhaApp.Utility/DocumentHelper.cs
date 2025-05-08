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
        private static string FilePath = "D:\\MagdyNudes";

        public static async Task<string> HandleImages(IFormFile imageFile, string oldImageFileName = null)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is empty.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Unsupported file format.");

            Directory.CreateDirectory(FilePath);

            var fileName = Guid.NewGuid() + extension;
            var filePath = Path.Combine(FilePath, fileName);

            // Delete old image if exists
            if (!string.IsNullOrWhiteSpace(oldImageFileName))
            {
                var oldPath = Path.Combine(FilePath, oldImageFileName);
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return fileName; 
        }
    }
}
