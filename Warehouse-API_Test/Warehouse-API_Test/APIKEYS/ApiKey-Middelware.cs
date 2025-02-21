namespace Warehouse_API_Test.APIKEYS
{
    public class ApiKey_Middelware
    {
        private readonly RequestDelegate _next;
        private readonly ApiKey _apiKeyService;

        public ApiKey_Middelware(RequestDelegate next, ApiKey apiKeyService)
        {
            _next = next;
            _apiKeyService = apiKeyService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401; 
                await context.Response.WriteAsync(" API-Key fehlt! Bitte stelle sicher, dass du einen gültigen API-Key im Header übergibst.");
                return;
            }

           
            if (!_apiKeyService.ValidateApiKey(extractedApiKey, out string newKey))
            {
                context.Response.StatusCode = 403; 
                await context.Response.WriteAsync($" Ungültiger API-Key oder Anfrageanzahl überschritten! Aktueller API-Key: {extractedApiKey}");
                return;
            }

            
            if (!string.IsNullOrEmpty(newKey))
            {
                context.Response.Headers.Append("X-New-Api-Key", newKey);
            }

           
            await _next(context);
        }
    }
}
