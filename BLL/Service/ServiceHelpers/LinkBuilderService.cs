using Microsoft.AspNetCore.Http;

namespace BLL.Service.ServiceHelpers;

public class LinkBuilderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LinkBuilderService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetBaseUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null) return string.Empty;

        return $"{request.Scheme}://{request.Host}{request.PathBase}";
    }
}