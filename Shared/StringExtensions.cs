namespace Microsoft
{
    using static System.Globalization.CultureInfo;

    static class StringExtensions
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        internal static string FormatInvariant( this string format, params object?[] args ) => string.Format( InvariantCulture, format, args );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        internal static string FormatDefault( this string format, params object?[] args ) => string.Format( CurrentCulture, format, args );
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}