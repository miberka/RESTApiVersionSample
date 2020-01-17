namespace Microsoft.AspNetCore.Mvc
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using System;
    using static Microsoft.AspNetCore.Http.StatusCodes;

    static class IErrorResponseProviderExtensions
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        internal static IActionResult BadRequest( this IErrorResponseProvider responseProvider, HttpContext context, string code, string message, string? messageDetail = null ) =>
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            responseProvider.CreateResponse( new ErrorResponseContext( context.Request, Status400BadRequest, code, message, messageDetail ) );

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        internal static IActionResult MethodNotAllowed( this IErrorResponseProvider responseProvider, HttpContext context, string code, string message, string? messageDetail = null ) =>
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            responseProvider.CreateResponse( new ErrorResponseContext( context.Request, Status405MethodNotAllowed, code, message, messageDetail ) );
    }
}