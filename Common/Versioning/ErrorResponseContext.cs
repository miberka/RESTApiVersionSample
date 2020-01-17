#if WEBAPI
namespace Microsoft.Web.Http.Versioning
#else
namespace Microsoft.AspNetCore.Mvc.Versioning
#endif
{
#if WEBAPI
    using System.Net;
#endif

    /// <summary>
    /// Represents the contextual information used when generating HTTP error responses related to API versioning.
    /// </summary>
    public partial class ErrorResponseContext
    {
        /// <summary>
        /// Gets the associated HTTP status code.
        /// </summary>
        /// <value>The associated HTTP status code.</value>
#if WEBAPI
        public HttpStatusCode StatusCode { get; }
#else
        public int StatusCode { get; }
#endif

        /// <summary>
        /// Gets the associated error code.
        /// </summary>
        /// <value>The associated error code.</value>
        public string ErrorCode { get; }

        /// <summary>
        /// Gets the associated error message.
        /// </summary>
        /// <value>The error message.</value>
        public string Message { get; }

        /// <summary>
        /// Gets the detailed error message.
        /// </summary>
        /// <value>The detailed error message, if any.</value>
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? MessageDetail { get; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}