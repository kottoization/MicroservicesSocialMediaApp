using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

public class JwtAuthorizationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var token = user?.Claims.FirstOrDefault(c => c.Type == "JWT")?.Value;

        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"JWT Token found: {token}"); // Logowanie tokenu
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            Console.WriteLine("JWT Token not found in user claims."); // Log, jeśli brak tokenu
        }

        Console.WriteLine($"Request Headers: {request.Headers}");

        return await base.SendAsync(request, cancellationToken);
    }
}

