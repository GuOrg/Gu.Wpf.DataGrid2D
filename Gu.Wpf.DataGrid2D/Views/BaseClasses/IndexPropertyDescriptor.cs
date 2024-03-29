namespace Gu.Wpf.DataGrid2D;

using System;
using System.ComponentModel;

internal abstract class IndexPropertyDescriptor : PropertyDescriptor
{
    private readonly Type elementType;

    protected IndexPropertyDescriptor(Type elementType, int index, bool isReadOnly)
        : base($"C{index}", null)
    {
        this.IsReadOnly = isReadOnly;
        this.elementType = elementType;
        this.Index = index;
    }

#pragma warning disable INPC017 // Backing field name must match.
    /// <inheritdoc/>
    public override Type ComponentType => this.elementType;
#pragma warning restore INPC017 // Backing field name must match.

    /// <inheritdoc/>
    public override bool IsReadOnly { get; }

#pragma warning disable INPC017 // Backing field name must match.
    /// <inheritdoc/>
    public override Type PropertyType => this.elementType;
#pragma warning restore INPC017 // Backing field name must match.

    protected int Index { get; }

    /// <inheritdoc/>
    public override bool CanResetValue(object component) => false;

    /// <inheritdoc/>
    public override void ResetValue(object component)
    {
        // NOP
    }

    /// <inheritdoc/>
    public override bool ShouldSerializeValue(object component)
    {
        return true;
    }
}
