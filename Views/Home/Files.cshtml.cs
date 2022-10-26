using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PeninsulaPhysiotherapy.Services;

namespace PeninsulaPhysiotherapy.Views.Home
{
    public class FilesModel : PageModel
    {
        private readonly ILogger<FilesModel> _logger;
        private readonly IFileUploadService _uploadService;
        public string FilePath;
        
        public FilesModel(ILogger<FilesModel> logger, IFileUploadService fileUploadService)
        {
            _logger = logger;
            _uploadService = fileUploadService;
        }

        public void OnGet()
        {
        }
        public async void OnPost(IFormFile file)
        {
            if (file != null)
            {
                FilePath=await _uploadService.UploadFileAsync(file);
            }
            
        }
    }
}
