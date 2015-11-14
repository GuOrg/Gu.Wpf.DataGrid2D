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
            Index = index;
            HeaderStringFormat = dataGrid.GetHeaderStringFormat();
            HeaderTemplate = dataGrid.GetHeaderTemplate();
            HeaderTemplateSelector = dataGrid.GetHeaderTemplateSelector();

            //Bind(this, CellTemplateProperty, dataGrid, GetPath(CellTemplateProperty));
            //Bind(this, CellTemplateSelectorProperty, dataGrid, GetPath(CellTemplateSelectorProperty));
            //Bind(this, CellEditingTemplateProperty, dataGrid, GetPath(CellEditingTemplateProperty));
            //Bind(this, CellEditingTemplateSelectorProperty, dataGrid, GetPath(CellEditingTemplateSelectorProperty));
            CellTemplate = dataGrid.GetCellTemplate();
            CellTemplateSelector = dataGrid.GetCellTemplateSelector();
            CellEditingTemplate = dataGrid.GetCellEditingTemplate();
            CellEditingTemplateSelector = dataGrid.GetCellEditingTemplateSelector();
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
            SetDataContext(cell, dataItem);
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
                    .OneWayTo(dataItem, Index);
                return;
            }

            var enumerable = (IEnumerable<object>)dataItem;
            cell.DataContext = enumerable.ElementAt(Index);
        }
    }
}