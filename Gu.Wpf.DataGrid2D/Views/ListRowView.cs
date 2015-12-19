namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class ListRowView : RowView<IEnumerable<IEnumerable>>, INotifyPropertyChanged
    {
        internal ListRowView(IEnumerable<IEnumerable> source, int index, Type elementType, PropertyDescriptorCollection properties, bool isTransposed)
            : base(source, index, properties, isTransposed)
        {
            this.ElementType = elementType;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal Type ElementType { get; }

        internal void RaiseAllChanged()
        {
            foreach (PropertyDescriptor property in this.GetProperties())
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
