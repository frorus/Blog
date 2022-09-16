namespace Blog.Extensions
{
    public interface IFileService
    {
        public Task<string> UploadImage(IFormFile file);
    }
}