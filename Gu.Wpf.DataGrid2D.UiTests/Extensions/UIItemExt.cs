namespace Gu.Wpf.DataGrid2D.UiTests
{
    using System;
    using System.Windows.Automation;
    using TestStack.White.UIItems;

    public static class UIItemExt
    {
        public static void DumpSupportedPatterns(this IUIItem item)
        {
            var patterns = item.AutomationElement.GetSupportedPatterns();
            foreach (var pattern in patterns)
            {
                Console.WriteLine(pattern.ProgrammaticName);
            }
        }

        public static void DumpSupportedProperties(this IUIItem item)
        {
            var properties = item.AutomationElement.GetSupportedProperties();
            foreach (var property in properties)
            {
                var value = item.AutomationElement.GetCurrentPropertyValue(property);
                Console.WriteLine($"{property.ProgrammaticName}: {value}");
            }
        }

        public static string ItemStatus(this IUIItem item)
        {
            return (string)item.AutomationElement.GetCurrentPropertyValue(AutomationElementIdentifiers.ItemStatusProperty, true);
        }
    }
}