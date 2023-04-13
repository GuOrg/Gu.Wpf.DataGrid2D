namespace Gu.Wpf.DataGrid2D;

using System;
using System.ComponentModel;

/// <summary>
/// A bindable representation for <typeparamref name="TSource"/>.
/// </summary>
/// <typeparam name="TSource">The source type.</typeparam>
public class RowView<TSource> : CustomTypeDescriptor
{
    private readonly PropertyDescriptorCollection properties;

    /// <summary>
    /// Initializes a new instance of the <see cref="RowView{TSource}"/> class.
    /// </summary>
    /// <param name="source">The <typeparamref name="TSource"/>.</param>
    /// <param name="index">The index.</param>
    /// <param name="properties">The <see cref="PropertyDescriptorCollection"/>.</param>
    protected RowView(TSource source, int index, PropertyDescriptorCollection properties)
    {
        this.Source = source;
        this.Index = index;
        this.properties = properties;
    }

    /// <summary>
    /// Gets the index.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets the count.
    /// </summary>
    public int Count => this.properties.Count;

    internal TSource Source { get; }

    /// <inheritdoc />
    public override string GetClassName() => this.GetType().FullName!;

    /// <inheritdoc />
    public override PropertyDescriptorCollection GetProperties() => this.properties;

    /// <inheritdoc />
    public override PropertyDescriptorCollection GetProperties(Attribute[] attributes) => this.properties;
}
