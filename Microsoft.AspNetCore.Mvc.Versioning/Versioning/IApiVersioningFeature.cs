namespace Microsoft.AspNetCore.Mvc.Versioning
{
    using System;

    /// <summary>
    /// Defines the behavior of the API versioning feature.
    /// </summary>
    [CLSCompliant( false )]
    public interface IApiVersioningFeature
    {
        /// <summary>
        /// Gets or sets the raw, unparsed API version for the current request.
        /// </summary>
        /// <value>The unparsed API version value for the current request.</value>
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        string? RawRequestedApiVersion { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        /// <summary>
        /// Gets or sets the API version for the current request.
        /// </summary>
        /// <value>The current <see cref="ApiVersion">API version</see> for the current request.</value>
        /// <remarks>If an API version was not provided for the current request or the value
        /// provided is invalid, this property will return <c>null</c>.</remarks>
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        ApiVersion? RequestedApiVersion { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

        /// <summary>
        /// Gets the action selection result associated with the current request.
        /// </summary>
        /// <value>The <see cref="ActionSelectionResult">action selection result</see> associated with the current request.</value>
        ActionSelectionResult SelectionResult { get; }
    }
}