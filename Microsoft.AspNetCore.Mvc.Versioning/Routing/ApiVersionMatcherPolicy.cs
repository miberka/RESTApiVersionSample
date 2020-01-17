﻿namespace Microsoft.AspNetCore.Mvc.Routing
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Routing.Matching;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Microsoft.AspNetCore.Mvc.Versioning.ApiVersionMapping;
    using static Microsoft.AspNetCore.Mvc.Versioning.ErrorCodes;
    using static System.Threading.Tasks.Task;

    /// <summary>
    /// Represents the <see cref="IEndpointSelectorPolicy">endpoint selector policy</see> for API versions.
    /// </summary>
    [CLSCompliant( false )]
    public sealed class ApiVersionMatcherPolicy : MatcherPolicy, IEndpointSelectorPolicy
    {
        readonly IOptions<ApiVersioningOptions> options;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiVersionMatcherPolicy"/> class.
        /// </summary>
        /// <param name="options">The <see cref="ApiVersioningOptions">options</see> associated with the action selector.</param>
        /// <param name="reportApiVersions">The <see cref="IReportApiVersions">object</see> used to report API versions.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory">factory</see> used to create <see cref="ILogger">loggers</see>.</param>
        public ApiVersionMatcherPolicy(
            IOptions<ApiVersioningOptions> options,
            IReportApiVersions reportApiVersions,
            ILoggerFactory loggerFactory )
        {
            this.options = options;
            ApiVersionReporter = reportApiVersions;
            Logger = loggerFactory.CreateLogger( GetType() );
        }

        /// <inheritdoc />
        public override int Order => 0;

        ApiVersioningOptions Options => options.Value;

        IApiVersionSelector ApiVersionSelector => Options.ApiVersionSelector;

        IReportApiVersions ApiVersionReporter { get; }

        ILogger Logger { get; }

        /// <inheritdoc />
        public bool AppliesToEndpoints( IReadOnlyList<Endpoint> endpoints )
        {
            if ( endpoints == null )
            {
                throw new ArgumentNullException( nameof( endpoints ) );
            }

            for ( var i = 0; i < endpoints.Count; i++ )
            {
                var action = endpoints[i].Metadata?.GetMetadata<ActionDescriptor>();

                if ( action?.GetProperty<ApiVersionModel>() != null )
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public Task ApplyAsync( HttpContext httpContext, CandidateSet candidates )
        {
            if ( httpContext == null )
            {
                throw new ArgumentNullException( nameof( httpContext ) );
            }

            if ( candidates == null )
            {
                throw new ArgumentNullException( nameof( candidates ) );
            }

            if ( IsRequestedApiVersionAmbiguous( httpContext, out var apiVersion ) )
            {
                return CompletedTask;
            }

            if ( apiVersion == null && Options.AssumeDefaultVersionWhenUnspecified )
            {
                apiVersion = TrySelectApiVersion( httpContext, candidates );
                httpContext.Features.Get<IApiVersioningFeature>().RequestedApiVersion = apiVersion;
            }

            var finalMatches = EvaluateApiVersion( candidates, apiVersion );

            if ( finalMatches.Count == 0 )
            {
                httpContext.SetEndpoint( ClientError( httpContext, candidates ) );
            }
            else
            {
                for ( var i = 0; i < finalMatches.Count; i++ )
                {
                    var (index, _, valid) = finalMatches[i];
                    candidates.SetValidity( index, valid );
                }
            }

            return CompletedTask;
        }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        static IReadOnlyList<(int Index, ActionDescriptor Action, bool Valid)> EvaluateApiVersion( CandidateSet candidates, ApiVersion? apiVersion )
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            var bestMatches = new List<(int Index, ActionDescriptor Action, bool)>();
            var implicitMatches = new List<(int, ActionDescriptor, bool)>();

            for ( var i = 0; i < candidates.Count; i++ )
            {
                ref var candidate = ref candidates[i];
                var action = candidate.Endpoint.Metadata?.GetMetadata<ActionDescriptor>();

                if ( action == null )
                {
                    continue;
                }

                // remember whether the candidate is currently valid. a matching api version will not
                // make the candidate valid; however, we want to short-circuit with 400 if no candidates
                // match the api version at all.
                switch ( action.MappingTo( apiVersion ) )
                {
                    case Explicit:
                        bestMatches.Add( (i, action, candidates.IsValidCandidate( i )) );
                        break;
                    case Implicit:
                        implicitMatches.Add( (i, action, candidates.IsValidCandidate( i )) );
                        break;
                }

                // perf: always make the candidate invalid so we only need to loop through the
                // final, best matches for any remaining candidates
                candidates.SetValidity( i, false );
            }

            switch ( bestMatches.Count )
            {
                case 0:
                    bestMatches.AddRange( implicitMatches );
                    break;
                case 1:
                    var model = bestMatches[0].Action.GetApiVersionModel();

                    if ( model.IsApiVersionNeutral )
                    {
                        bestMatches.AddRange( implicitMatches );
                    }

                    break;
            }

            return bestMatches.ToArray();
        }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        bool IsRequestedApiVersionAmbiguous( HttpContext httpContext, out ApiVersion? apiVersion )
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            try
            {
                apiVersion = httpContext.GetRequestedApiVersion();
            }
            catch ( AmbiguousApiVersionException ex )
            {
                Logger.LogInformation( ex.Message );
                apiVersion = default;

                var handlerContext = new RequestHandlerContext( Options.ErrorResponses )
                {
                    Code = AmbiguousApiVersion,
                    Message = ex.Message,
                };

                httpContext.SetEndpoint( new BadRequestHandler( handlerContext ) );
                return true;
            }

            return false;
        }

        ApiVersion TrySelectApiVersion( HttpContext httpContext, CandidateSet candidates )
        {
            var models = new List<ApiVersionModel>();

            for ( var i = 0; i < candidates.Count; i++ )
            {
                ref var candidate = ref candidates[i];
                var model = candidate.Endpoint.Metadata?.GetMetadata<ActionDescriptor>()?.GetApiVersionModel();

                if ( model != null )
                {
                    models.Add( model );
                }
            }

            return ApiVersionSelector.SelectVersion( httpContext.Request, models.Aggregate() );
        }

        RequestHandler ClientError( HttpContext httpContext, CandidateSet candidateSet )
        {
            var candidates = new List<ActionDescriptor>( candidateSet.Count );

            for ( var i = 0; i < candidateSet.Count; i++ )
            {
                ref var candidate = ref candidateSet[i];
                var action = candidate.Endpoint.Metadata?.GetMetadata<ActionDescriptor>();

                if ( action != null )
                {
                    candidates.Add( action );
                }
            }

            var builder = new ClientErrorBuilder()
            {
                Options = Options,
                ApiVersionReporter = ApiVersionReporter,
                HttpContext = httpContext,
                Candidates = candidates,
                Logger = Logger,
            };

            return builder.Build();
        }
    }
}