namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class ListRowView : RowView<IEnumerable<IEnumerable>>
    {
        internal ListRowView(IEnumerable<IEnumerable> source, int index, PropertyDescriptorCollection properties, bool isTransposed)
            : base(source, index, properties, isTransposed)
        {
        }
    }
}
