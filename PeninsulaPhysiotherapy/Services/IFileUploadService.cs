namespace PeninsulaPhysiotherapy.Services

{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
