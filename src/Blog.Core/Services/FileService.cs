using Blog.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Blog.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImage(IFormFile file)
        {
            long totalBytes = file.Length;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            byte[] buffer = new byte[16 * 1024];

            using (FileStream output = File.Create(GetPathAndFileName(fileName)))
            {
                using (Stream input = file.OpenReadStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalBytes += readBytes;
                    }
                }
            }

            return fileName;
        }

        private string GetPathAndFileName(string fileName)
        {
            string path = _environment.WebRootPath + "\\img\\avatars\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path + fileName;
        }
    }
}