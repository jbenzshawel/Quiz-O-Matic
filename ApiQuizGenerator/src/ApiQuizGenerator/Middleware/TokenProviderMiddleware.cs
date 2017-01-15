// using System;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Options;
// using ApiQuizGenerator.AppClasses;
// using ApiQuizGenerator.Models.ManageViewModels;
// using Microsoft.AspNetCore.Identity;
// using ApiQuizGenerator.Models;

// namespace ApiQuizGenerator.Middleware
// {
//     public class TokenProviderMiddleware
//     {
//         private readonly RequestDelegate _next;
//         private readonly TokenProviderOptions _options;
 
//         private TokenProvider _TokenProvider { get; set; }


//         public TokenProviderMiddleware(
//             RequestDelegate next,
//             IOptions<TokenProviderOptions> options)
//         {
//             _next = next;
//             _options = options.Value;
            
//             _TokenProvider = new TokenProvider();
//         }
 
//         public Task Invoke(HttpContext context)
//         {
//             // If the request path doesn't match, skip
//             if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
//             {
//                 return _next(context);
//             }
   
//             // Request must be POST with Content-Type: application/x-www-form-urlencoded
//             if (!context.Request.Method.Equals("POST")
//                || !context.Request.HasFormContentType)
//             {
//                 context.Response.StatusCode = 400;
//                 return context.Response.WriteAsync("Bad request.");
//             }
//             return _TokenProvider.GenerateToken(context, context.User.Identity.Name);
//         }
//     }
// }