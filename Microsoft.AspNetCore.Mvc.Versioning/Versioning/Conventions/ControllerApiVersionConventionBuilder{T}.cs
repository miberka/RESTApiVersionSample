﻿namespace Microsoft.AspNetCore.Mvc.Versioning.Conventions
{
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using System;
    using System.Reflection;

    /// <content>
    /// Provides additional implementation specific to Microsoft ASP.NET Core.
    /// </content>
    /// <typeparam name="T">The <see cref="Type">type</see> of <see cref="ICommonModel">model</see>.</typeparam>
    [CLSCompliant( false )]
    public partial class ControllerApiVersionConventionBuilder<T> where T : notnull
    {
        /// <summary>
        /// Attempts to get the convention for the specified action method.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo">method</see> representing the action to retrieve the convention for.</param>
        /// <param name="convention">The retrieved <see cref="IApiVersionConvention{T}">convention</see> or <c>null</c>.</param>
        /// <returns>True if the convention was successfully retrieved; otherwise, false.</returns>
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        protected override bool TryGetConvention( MethodInfo method, out IApiVersionConvention<ActionModel>? convention )
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            if ( ActionBuilders.TryGetValue( method, out var actionBuilder ) )
            {
                convention = actionBuilder;
                return true;
            }

            convention = default;
            return false;
        }
    }
}