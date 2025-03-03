using System.Security.Claims;

namespace ProfileService.Middlewares
{
    public class UserIdentificationMiddleware
    {

        private readonly RequestDelegate _next;

        public UserIdentificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task  Invoke(HttpContext context)
        {
			if (context.User.Identity?.IsAuthenticated == true)
			{
				var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
				Console.WriteLine($"User is Authenticated: {context.User.Identity.Name}"); // Debugging
				Console.WriteLine($"User ID Claim: {idClaim?.Value}"); // Debugging

				if (idClaim != null)
				{
					context.Items["UserId"] = idClaim.Value;
					Console.WriteLine("User ID stored in HttpContext.Items");
				}
				else
				{
					Console.WriteLine("User ID claim not found in JWT token.");
				}
			}

			await _next(context);

        }
    }
    
}
