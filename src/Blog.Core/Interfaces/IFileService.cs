using Microsoft.AspNetCore.Http;

namespace Blog.Core.Interfaces
{
    public interface IFileService
    {
        public Task<string> UploadImage(IFormFile file);
    }
}