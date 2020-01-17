namespace Microsoft.AspNetCore.Mvc.Versioning
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    sealed class RequestHandlerContext
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        readonly IReportApiVersions? reporter;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        readonly Lazy<ApiVersionModel>? apiVersions;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        internal RequestHandlerContext( IErrorResponseProvider errorResponseProvider )
            : this( errorResponseProvider, null, null ) { }

        internal RequestHandlerContext(
            IErrorResponseProvider errorResponseProvider,
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            IReportApiVersions? reportApiVersions,
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            Lazy<ApiVersionModel>? apiVersions )
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            ErrorResponses = errorResponseProvider;
            reporter = reportApiVersions;
            this.apiVersions = apiVersions;
        }

        internal IErrorResponseProvider ErrorResponses { get; }

        internal string Message { get; set; } = string.Empty;

        internal string Code { get; set; } = string.Empty;

        internal string[] AllowedMethods { get; set; } = Array.Empty<string>();

        internal IList<object> Metadata { get; set; } = Array.Empty<object>();

        internal void ReportApiVersions( HttpResponse response )
        {
            if ( reporter != null && apiVersions != null )
            {
                reporter.Report( response.Headers, apiVersions );
            }
        }
    }
}