using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using PeninsulaPhysiotherapy.Services;

namespace PeninsulaPhysiotherapy.Services
{
    public class LocalFileUploadService:IFileUploadService
    {
        private readonly IWebHostEnvironment environment;
        public LocalFileUploadService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var filePath = Path.Combine(environment.ContentRootPath,@"wwwroot\storage",file.FileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return filePath;

        }
    }
}
