namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Windows;
    using System.Windows.Controls;

    public class CellEditingTemplateSelectorExample : DataTemplateSelector
    {
        public DataTemplate FirstTemplate { get; set; }

        public DataTemplate SecondTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if (item is CellTemplateDemoClass)
            {
                CellTemplateDemoClass democlass = item as CellTemplateDemoClass;
                if ((democlass.Value1 + democlass.Value2) > 5)
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
