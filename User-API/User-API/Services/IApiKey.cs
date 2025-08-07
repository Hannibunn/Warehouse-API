namespace User_API.Services
{
    public interface  IApiKey
    {
        bool IsValid(string key);
        string GenerateKey();
    }
}
