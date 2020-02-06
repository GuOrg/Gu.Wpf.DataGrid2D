namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public class CellTemplateSelectorExample : DataTemplateSelector
    {
        public DataTemplate? FirstTemplate { get; set; }

        public DataTemplate? SecondTemplate { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            if (item is CellTemplateDemoClass democlass)
            {
                if (democlass.Value1 > 2)
                {
                    return this.FirstTemplate;
                }
                else
                {
                    return this.SecondTemplate;
                }
            }

            return null;
        }
    }
}
