namespace Gu.Wpf.DataGrid2D.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;

    public class CodeGenTests
    {
        public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.RegisterAttached(
            "CellTemplate",
            typeof(DataTemplate),
            typeof(Source2D),
            new FrameworkPropertyMetadata(default(DataTemplate)));
       
        [Test]
        public void GenerateDataGridTemplateColumnAttachedPropertyFields()
        {
            IEnumerable ints = new int[2, 2];
            DumpFields(typeof(Source2D), GetDpFields(typeof(DataGridTemplateColumn)));
        }

        [Test]
        public void GenerateDataGridColumnAttachedPropertyFields()
        {
            var headerFields = GetDpFields(typeof (DataGridColumn)).Where(x => x.Name.StartsWith("Header")).ToArray();
            DumpFields(typeof(Source2D), headerFields);
        }

        [Test]
        public void GenerateDataGridTemplateColumnMethods()
        {
            DumpMethods(GetDpFields(typeof(DataGridTemplateColumn)));
        }

        [Test]
        public void GenerateDataGridColumnMethods()
        {
            var headerFields = GetDpFields(typeof(DataGridColumn)).Where(x => x.Name.StartsWith("Header")).ToArray();
            DumpMethods(headerFields);
        }

        [Test]
        public void DumpDataGridTemplateColumnBindingsTest()
        {
            var readOnlyList = GetDpFields(typeof(DataGridTemplateColumn));
            foreach (var fieldInfo in readOnlyList)
            {
                Console.WriteLine("            Bind(this, {0}, dataGrid, GetPath({0}));",fieldInfo.Name);
            }
        }

        [Test]
        public void DumpDataGridColumnBindingsTest()
        {
            var readOnlyList = GetDpFields(typeof(DataGridColumn)).Where(f=>f.Name.StartsWith("Header"));
            foreach (var fieldInfo in readOnlyList)
            {
                Console.WriteLine("            Bind(this, {0}, dataGrid, GetPath({0}));", fieldInfo.Name);
            }
        }

        //public static void SetCellTemplate(this DataGrid element, DataTemplate value)
        //{
        //    element.SetValue(CellTemplateProperty, value);
        //}

        //[AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        //[AttachedPropertyBrowsableForType(typeof(DataGrid))]
        //public static DataTemplate GetCellTemplate(this DataGrid element)
        //{
        //    return (DataTemplate)element.GetValue(CellTemplateProperty);
        //}

        private static IReadOnlyList<FieldInfo> GetDpFields(Type from)
        {
            var dps = from.GetFields(BindingFlags.Static | BindingFlags.Public)
                          .Where(f => f.FieldType == typeof(DependencyProperty));

            return dps.ToArray();
        }

        private static void DumpFields(Type owner, IEnumerable<FieldInfo> dps)
        {
            foreach (var info in dps)
            {
                var dp = (DependencyProperty)info.GetValue(null);
                var name = info.Name.Replace("Property", "");
                Console.WriteLine(
                    "    public static readonly DependencyProperty {0}Property = DependencyProperty.RegisterAttached(", name);
                Console.WriteLine(@"       ""{0}"",", name);
                Console.WriteLine(@"       typeof({0}),", dp.PropertyType.Name);
                Console.WriteLine(@"       typeof({0}),", owner.Name);
                Console.WriteLine(@"       new FrameworkPropertyMetadata(default({0})));", dp.PropertyType.Name);

                Console.WriteLine();
            }
        }

        private static void DumpMethods(IEnumerable<FieldInfo> dps)
        {
            foreach (var info in dps)
            {
                var dp = (DependencyProperty)info.GetValue(null);
                var name = info.Name.Replace("Property", "");
                Console.WriteLine("    public static void Set{0}(this DataGrid element, {1} value)", name, dp.PropertyType.Name);
                Console.WriteLine("    {");
                Console.WriteLine(@"       element.SetValue({0}, value);", info.Name);
                Console.WriteLine("    }");
                Console.WriteLine();

                Console.WriteLine("    [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]");
                Console.WriteLine("    [AttachedPropertyBrowsableForType(typeof(DataGrid))]");
                Console.WriteLine("    public static {0} Get{1}(this DataGrid element)", dp.PropertyType.Name, name);
                Console.WriteLine("    {");
                Console.WriteLine(@"       return ({0})element.GetValue({1});", dp.PropertyType.Name, info.Name);
                Console.WriteLine("    }");
                Console.WriteLine();
            }
        }
    }
}
