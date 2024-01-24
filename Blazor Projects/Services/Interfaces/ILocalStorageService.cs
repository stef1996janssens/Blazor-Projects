namespace Blazor_Projects.Services.Interfaces;

public interface ILocalStorageService
{
    Task<string> Get(string key);
    Task Set(string key, string value);
}
