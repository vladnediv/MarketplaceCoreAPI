using BLL.Model;
using BLL.Service.Model;
using Microsoft.AspNetCore.Http;

namespace BLL.Service.Interface;

public interface IFileService
{
    public Task<ServiceResponse<string>> SavePictureAsync(IFormFile file);
    public Task<ServiceResponse> DeletePictureAsync(string path);
}