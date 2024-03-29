namespace Gu.Wpf.DataGrid2D;

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

/// <summary>
/// A bindable view of a list row.
/// </summary>
public class ListRowView : RowView<IView2D>, INotifyPropertyChanged
{
    private static readonly EventDescriptorCollection Events = TypeDescriptor.GetEvents(typeof(ListRowView));

    internal ListRowView(IView2D source, int index, PropertyDescriptorCollection properties)
        : base(source, index, properties)
    {
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <inheritdoc />
    public override EventDescriptorCollection GetEvents() => Events;

    /// <inheritdoc />
    public override EventDescriptorCollection GetEvents(Attribute[] attributes) => Events;

    internal void RaiseAllChanged()
    {
        foreach (var property in this.GetProperties().Cast<PropertyDescriptor>())
        {
            this.OnPropertyChanged(property.Name);
        }
    }

    internal void RaiseColumnsChanged(int startColumn, int count)
    {
        var collection = this.GetProperties();
        for (int i = startColumn; i < startColumn + count; i++)
        {
            this.OnPropertyChanged(collection[i].Name);
        }
    }

    /// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
    /// <param name="propertyName">The name of the property.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
