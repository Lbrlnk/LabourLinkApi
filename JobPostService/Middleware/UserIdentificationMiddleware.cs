using System.Security.Claims;

namespace JobPostService.Middleware
{
    public class UserIdentificationMiddleware
    {


        private readonly RequestDelegate _next;

        public UserIdentificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

                if (idClaim != null)
                {
                    context.Items["UserId"] = idClaim.Value;

                }


            }
            await _next(context);

        }

    }
}
