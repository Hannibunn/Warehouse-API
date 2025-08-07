namespace User_API.Services
{
    public class ApiKeyService : IApiKey
    {
        private readonly HashSet<string> _validKeys = new()
        {
            "12345", // Beispielkeys, in DB oder Config speichern
            "ABCDE"
        };

        public bool IsValid(string key)
        {
            return _validKeys.Contains(key);
        }

        public string GenerateKey()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
