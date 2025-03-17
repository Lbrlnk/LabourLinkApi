namespace ProfileService.Middlewares
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
            // Extract the access token from cookies
            var accessToken = context.Request.Cookies["accessToken"];
            Console.WriteLine($"Extracted Token: {accessToken}");

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Add the token to the Authorization header
                context.Request.Headers["Authorization"] = $"Bearer {accessToken}";
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
