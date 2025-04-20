namespace AuthenticationService.Middleware
{
    public class TokenAccessingMiddleware
    {

        private readonly RequestDelegate _next;
        public TokenAccessingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var accessToken = context.Request.Cookies["accessToken"];
            Console.WriteLine($"Extracted Token: {accessToken}");

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Request.Headers["Authorization"] = $"Bearer {accessToken}";
            }

            await _next(context);
        }
    }
}
