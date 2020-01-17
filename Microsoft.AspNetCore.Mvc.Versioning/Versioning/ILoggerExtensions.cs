namespace Microsoft.AspNetCore.Mvc.Versioning
{
    using Microsoft.AspNetCore.Mvc.ActionConstraints;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Extensions.Logging.LoggerMessage;
    using static Extensions.Logging.LogLevel;
    using static System.Globalization.CultureInfo;

    static class ILoggerExtensions
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        static readonly Action<ILogger, string, Exception?> ambiguousActions =
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            Define<string>( Error, 1, "Request matched multiple actions resulting in ambiguity. Matching actions: {AmbiguousActions}" );

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        static readonly Action<ILogger, string, string, IActionConstraint, Exception?> constraintMismatch =
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            Define<string, string, IActionConstraint>( Debug, 2, "Action '{ActionName}' with id '{ActionId}' did not match the constraint '{ActionConstraint}'" );

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        static readonly Action<ILogger, string, Exception?> apiVersionUnspecified =
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            Define<string>( Information, 3, "Request did not specify a service API version, but multiple candidate actions were found. Candidate actions: {CandidateActions}" );

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        static readonly Action<ILogger, ApiVersion?, string, Exception?> apiVersionUnspecifiedWithDefaultVersion =
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            Define<ApiVersion?, string>( Information, 4, "Request did not specify a service API version, but multiple candidate actions were found; however, none matched the selected default API version '{ApiVersion}'. Candidate actions: {CandidateActions}" );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        static readonly Action<ILogger, ApiVersion?, string, Exception?> apiVersionUnmatched =
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            Define<ApiVersion?, string>( Information, 5, "Multiple candidate actions were found, but none matched the requested service API version '{ApiVersion}'. Candidate actions: {CandidateActions}" );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        static readonly Action<ILogger, string?, Exception?> apiVersionInvalid =
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            Define<string?>( Information, 6, "Request contained the service API version '{ApiVersion}', which is not valid" );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        static readonly Action<ILogger, string[]?, Exception?> noActionsMatched =
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            Define<string[]?>( Debug, 3, "No actions matched the current request. Route values: {RouteValues}" );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        internal static void AmbiguousActions( this ILogger logger, string actionNames ) => ambiguousActions( logger, actionNames, null );

        internal static void ConstraintMismatch( this ILogger logger, string actionName, string actionId, IActionConstraint actionConstraint ) =>
            constraintMismatch( logger, actionName, actionId, actionConstraint, null );

        internal static void ApiVersionUnspecified( this ILogger logger, string actionNames ) => apiVersionUnspecified( logger, actionNames, null );

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        internal static void ApiVersionUnspecified( this ILogger logger, ApiVersion? apiVersion, string actionNames ) => apiVersionUnspecifiedWithDefaultVersion( logger, apiVersion, actionNames, null );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        internal static void ApiVersionUnmatched( this ILogger logger, ApiVersion? apiVersion, string actionNames ) => apiVersionUnmatched( logger, apiVersion, actionNames, null );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        internal static void ApiVersionInvalid( this ILogger logger, string? apiVersion ) => apiVersionInvalid( logger, apiVersion, null );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        internal static void NoActionsMatched( this ILogger logger, IDictionary<string, object> routeValueDictionary )
        {
            if ( !logger.IsEnabled( Debug ) )
            {
                return;
            }

            var routeValues = default( string[] );

            if ( routeValueDictionary != null )
            {
                routeValues = routeValueDictionary.Select( pair => pair.Key + "=" + Convert.ToString( pair.Value, InvariantCulture ) ).ToArray();
            }

            noActionsMatched( logger, routeValues, null );
        }
    }
}