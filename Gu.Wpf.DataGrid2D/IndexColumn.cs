namespace Gu.Wpf.DataGrid2D
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class IndexColumn : DataGridTemplateColumn
    {
        public IndexColumn(DataGrid dataGrid, int index)
        {
            this.Index = index;
            this.HeaderStringFormat = dataGrid.GetHeaderStringFormat();
            this.HeaderTemplate = dataGrid.GetHeaderTemplate();
            this.HeaderTemplateSelector = dataGrid.GetHeaderTemplateSelector();

            this.CellTemplate = dataGrid.GetCellTemplate();
            this.CellTemplateSelector = dataGrid.GetCellTemplateSelector();
            this.CellEditingTemplate = dataGrid.GetCellEditingTemplate();
            this.CellEditingTemplateSelector = dataGrid.GetCellEditingTemplateSelector();
        }

        /// <summary>
        /// IEnumerable here relies on validation previously
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="headers"></param>
        /// <param name="index"></param>
        internal IndexColumn(DataGrid dataGrid, IEnumerable headers, int index)
            : this(dataGrid, index)
        {
            this.Bind(HeaderProperty)
                .OneWayTo(headers, index);
        }

        public int Index { get; }

        internal void BindHeader(IEnumerable headers, int index)
        {
            this.Bind(HeaderProperty)
                .OneWayTo(headers, index);
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            this.SetDataContext(cell, dataItem);
            var frameworkElement = base.GenerateElement(cell, dataItem);
            if (frameworkElement == null)
            {
                ContentPresenter contentPresenter = new ContentPresenter
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                BindingOperations.SetBinding(contentPresenter, ContentPresenter.ContentProperty, new Binding());
                return contentPresenter;
            }

            return frameworkElement;
        }

        private void SetDataContext(DataGridCell cell, object dataItem)
        {
            if (dataItem is IList || dataItem is IReadOnlyList<object>)
            {
                cell.Bind(FrameworkElement.DataContextProperty)
                    .OneWayTo(dataItem, this.Index);
                return;
            }

            var enumerable = (IEnumerable<object>)dataItem;
            cell.DataContext = enumerable.ElementAt(this.Index);
        }
    }
}