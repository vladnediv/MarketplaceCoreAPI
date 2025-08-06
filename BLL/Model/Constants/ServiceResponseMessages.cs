namespace BLL.Service.Model.Constants;

public static class ServiceResponseMessages
{
    public static string EntityNotFound(string entityType) => $"The entity [{entityType}] was not found.";
    public static string EntityNotFoundById(string entityName, int entityId) => $"The entity [{entityName}] was not found by id [{entityId}].";
    public static string ArgumentIsNull(string variable, string entityType) => $"The argument [{variable}] of type [{entityType}] is null.";
    public const string UnknownError = "An unknown error has occurred.";
    
    public static string AccessDenied(string entityType, int entityId) => $"Your access to entity [{entityType}] by id [{entityId}] is denied.";

    public static string ProductDeactivated(string productName) =>
        $"The product [{productName}] is currently deactivated.";
}