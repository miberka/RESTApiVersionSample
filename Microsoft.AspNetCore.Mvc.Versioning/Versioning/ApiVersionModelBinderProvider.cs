namespace Microsoft.AspNetCore.Mvc.Versioning
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;

    sealed class ApiVersionModelBinderProvider : IModelBinderProvider
    {
        static readonly Type ApiVersionType = typeof( ApiVersion );
        static readonly ApiVersionModelBinder binder = new ApiVersionModelBinder();

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public IModelBinder? GetBinder( ModelBinderProviderContext context )
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            if ( context == null )
            {
                throw new ArgumentNullException( nameof( context ) );
            }

            if ( ApiVersionType.IsAssignableFrom( context.Metadata.ModelType ) )
            {
                return binder;
            }

            return default;
        }
    }
}