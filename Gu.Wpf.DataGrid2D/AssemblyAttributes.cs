using System;
using System.Runtime.CompilerServices;
using System.Windows.Markup;

[assembly: CLSCompliant(false)]
[assembly: XmlnsDefinition("http://gu.se/DataGrid2D", "Gu.Wpf.DataGrid2D")]
[assembly: XmlnsPrefix("http://gu.se/DataGrid2D", "dataGrid2D")]
[assembly: InternalsVisibleTo("Gu.Wpf.DataGrid2D.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001000D790C1903DF2D575149C55D658639B3EBDD93809BC07A552E0AED21AC80BCF33448B1DE940C7DEE7A93FF2E77C2B720DD9A5C9D146D8FA01785B02A55BCC04030DB5A95BF54EC544235B70C6F224F414D823D9BA2DC6FFB9872EF1F41DD016DE00B8E0B692594F0CA9D03266BA36BFFD94382B48ED3F9CE826205938C44BBBC", AllInternalsVisible = true)]

#if NET45
#pragma warning disable SA1402, SA1502, SA1600, SA1649, GU0073
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class DisallowNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class MaybeNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class NotNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        public MaybeNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;

        public bool ReturnValue { get; }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;

        public bool ReturnValue { get; }
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        public NotNullIfNotNullAttribute(string parameterName) => this.ParameterName = parameterName;

        public string ParameterName { get; }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class DoesNotReturnIfAttribute : Attribute
    {
        public DoesNotReturnIfAttribute(bool parameterValue) => this.ParameterValue = parameterValue;

        public bool ParameterValue { get; }
    }
}
#endif
