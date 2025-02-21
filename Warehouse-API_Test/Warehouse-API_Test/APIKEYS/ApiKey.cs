using System.Security.Cryptography;

namespace Warehouse_API_Test.APIKEYS
{
    public class ApiKey
    {
        public int RequestCount { get; set; }
        private static readonly Dictionary<string, ApiKeyData> _apiKeys = new();
        private static readonly object _lock = new(); // Verhindert gleichzeitige Zugriffe

        private readonly string _adminKey;

        public ApiKey(IConfiguration configuration)
        {
            _adminKey = configuration["ApiKey:AdminKey"];  
        }

        public string GetAdminKey() => _adminKey;
        private string GenerateApiKey()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

       
        public string CreateApiKey()
        {
            var key = GenerateApiKey();
            lock (_lock)
            {
                if (!_apiKeys.ContainsKey(key))
                {
                    _apiKeys[key] = new ApiKeyData { RequestCount = 0, IsAdmin = false }; 
                }
            }
            return key;
        }

      
        public bool ValidateApiKey(string key, out string newKey)
        {
            newKey = null;
            lock (_lock)
            {
                
                if (key == _adminKey)
                {
                    newKey = _adminKey; 
                    return true;  
                }

               
                if (!_apiKeys.ContainsKey(key))
                {
                    return false;
                }

              
                _apiKeys[key].RequestCount++;

               
                if (_apiKeys[key].RequestCount >= 20)
                {
                    newKey = GenerateApiKey(); 
                    _apiKeys.Remove(key); 
                    _apiKeys[newKey] = new ApiKeyData { RequestCount = 0 }; 
                }
            }

            return true;
        }

       
        public IEnumerable<string> GetAllApiKeys()
        {
            lock (_lock)
            {
                return _apiKeys.Keys.ToList();
            }
        }

       
        private class ApiKeyData
        {
            public int RequestCount { get; set; }
            public bool IsAdmin { get; set; }
        }


    }


}

