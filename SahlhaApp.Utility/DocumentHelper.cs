using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SahlhaApp.Utility
{
    public class DocumentHelper
    {
        private static string FilePath = "C:\\Users\\pc\\Desktop\\Medicio\\Medicio\\assets\\img"; // Change the file path as needed

        // Handle a single file upload or update (delete old file if necessary)
        public static async Task<string> HandleSingleFile(IFormFile file, string oldFileName = null)
        {
            if (file == null || file.Length == 0) throw new ArgumentException("File is empty.");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(FilePath, fileName);

            // Delete old image if exists
            if (!string.IsNullOrWhiteSpace(oldFileName))
            {
                var oldFilePath = Path.Combine(FilePath, oldFileName);
                if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
            }

            using (var stream = File.Create(filePath)) await file.CopyToAsync(stream);


            return fileName;
        }

        // Handle multiple file uploads
        public static async Task<List<string>> HandleMultipleFiles(List<IFormFile> files)
        {
            if (files == null || !files.Any())
                throw new ArgumentException("No files uploaded.");

            Directory.CreateDirectory(FilePath);

            var fileNames = new List<string>();

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(FilePath, fileName);

                using (var stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                fileNames.Add(fileName);
            }

            return fileNames;
        }

        // Delete a file if it exists
        public static void DeleteFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name is empty.");

            var filePath = Path.Combine(FilePath, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);
        }

        // Update a file (delete the old file and upload the new one)
        public static async Task<string> UpdateFile(IFormFile newFile, string oldFileName)
        {
            // Delete the old file first
            DeleteFile(oldFileName);

            // Now handle the new file upload
            return await HandleSingleFile(newFile);
        }
    }
}
