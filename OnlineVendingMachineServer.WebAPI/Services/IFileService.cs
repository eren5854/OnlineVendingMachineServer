namespace OnlineVendingMachineServer.WebAPI.Services;

public interface IFileService
{
    string SaveFile(IFormFile file, string folderPath);
    string GetFileUrl(string fileName, string folderPath);
    string DeleteFile(string fileName, string folderPath);
    string UpdateFile(IFormFile file, string folderPath, string existingFileName);
}
