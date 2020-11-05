namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A transposed row.
    /// </summary>
    public class TransposedRow : CustomTypeDescriptor, INotifyPropertyChanged
    {
        private static readonly EventDescriptorCollection Events = TypeDescriptor.GetEvents(typeof(TransposedRow));
        private readonly PropertyDescriptorCollection properties;

        internal TransposedRow(
            TransposedItemsSource source,
            PropertyDescriptor property)
        {
            this.Source = source;
            this.Property = property;
            var count = source.Source?.Count() ?? 0;
            var propertyDescriptors = new PropertyDescriptor[count + 1];
            propertyDescriptors[0] = new NamePropertyDescriptor(property);
            for (int i = 0; i < count; i++)
            {
                propertyDescriptors[i + 1] = new TransposedPropertyDescriptor(i, property);
            }

            this.properties = new PropertyDescriptorCollection(propertyDescriptors, readOnly: true);
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        internal PropertyDescriptor Property { get; }

        internal TransposedItemsSource Source { get; }

        /// <inheritdoc />
        public override EventDescriptorCollection GetEvents() => Events;

        /// <inheritdoc />
        public override EventDescriptorCollection GetEvents(Attribute[] attributes) => Events;

        /// <inheritdoc />
        public override PropertyDescriptorCollection GetProperties() => this.properties;

        /// <inheritdoc />
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes) => this.properties;

        internal void RaiseColumnPropertyChanged(object sender)
        {
            var indexOf = this.Source.Source?.IndexOf(sender) ?? -1;
            if (indexOf < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sender));
            }

            this.OnPropertyChanged(this.properties[indexOf + 1].Name);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
