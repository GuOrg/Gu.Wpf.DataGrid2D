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

            Width = dataGrid.GetWidth();
            MinWidth = dataGrid.GetMinWidth();
            MaxWidth = dataGrid.GetMaxWidth();
            //Bind(this, CellTemplateProperty, dataGrid, GetPath(CellTemplateProperty));
            //Bind(this, CellTemplateSelectorProperty, dataGrid, GetPath(CellTemplateSelectorProperty));
            //Bind(this, CellEditingTemplateProperty, dataGrid, GetPath(CellEditingTemplateProperty));
            //Bind(this, CellEditingTemplateSelectorProperty, dataGrid, GetPath(CellEditingTemplateSelectorProperty));
            CellTemplate = dataGrid.GetCellTemplate();
            CellTemplateSelector = dataGrid.GetCellTemplateSelector();
            CellEditingTemplate = dataGrid.GetCellEditingTemplate();
            CellEditingTemplateSelector = dataGrid.GetCellEditingTemplateSelector();
        }

        public IndexColumn(DataGrid dataGrid, object[] headers, int index)
            : this(dataGrid, index)
        {
            BindHeader(headers, index);
        }

        public void BindHeader(object[] headers, int index)
        {
            Helpers.Bind(this, HeaderProperty, headers,index);
        }

        public int Index { get; private set; }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var list = dataItem as IList;
            if (list != null)
            {
                Helpers.Bind(cell, FrameworkElement.DataContextProperty, dataItem, Index);
            }
            else
            {
                var enumerable = (IEnumerable<object>)dataItem;
                cell.DataContext = enumerable.ElementAt(Index);
            }
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
    }
}