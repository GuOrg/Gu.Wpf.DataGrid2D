namespace Gu.Wpf.DataGrid2D.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;

    [Explicit]
    public class CodeGenTests
    {
        private readonly string[] excluded = { "HeaderProperty ", "HeaderStyleProperty", "ActualWidthProperty" };

        [TestCase(typeof(DataGridColumn), "Header")]
        [TestCase(typeof(DataGridColumn), "Width")]
        [TestCase(typeof(DataGridTemplateColumn), "")]
        public void GenerateAttachedPropertyFields(Type type, string contains)
        {
            var properties =
                GetDpFields(type)
                    .Where(x => x.Name.Contains(contains))
                    .Where(x => !this.excluded.Contains(x.Name))
                    .ToArray();
            DumpFields(typeof(Source2D), properties);
            DumpMetaData(properties);
        }

        [TestCase(typeof(DataGridColumn), "Header")]
        [TestCase(typeof(DataGridColumn), "Width")]
        [TestCase(typeof(DataGridTemplateColumn), "")]
        public void GenerateDataGridColumnMethods(Type type, string contains)
        {
            var properties =
                GetDpFields(type)
                    .Where(x => x.Name.Contains(contains))
                    .Where(x => !this.excluded.Contains(x.Name))
                    .ToArray();
            DumpMethods(properties);
        }

        [TestCase(false, typeof(DataGridColumn), "Header")]
        [TestCase(false, typeof(DataGridColumn), "Width")]
        [TestCase(false, typeof(DataGridTemplateColumn), "")]
        public void DumpDataGridColumnBindingsTest(bool bind, Type type, string contains)
        {
            var properties =
                GetDpFields(type)
                    .Where(x => x.Name.Contains(contains))
                    .Where(x => !this.excluded.Contains(x.Name))
                    .Select(x => x.Name)
                    .Distinct()
                    .Concat(this.excluded.Except(new[] { "ActualWidthProperty", "HeaderProperty" }))
                    .ToArray();
            foreach (var name in properties)
            {
                if (bind)
                {
                    Console.WriteLine("            Bind(this, {0}, dataGrid, GetPath({0}));", name);
                }
                else
                {
                    Console.WriteLine("            {0} = dataGrid.Get{0}();", name.Replace("Property", ""));
                }
            }
        }

        [TestCase(typeof(DataGridColumn))]
        [TestCase(typeof(DataGridTemplateColumn))]
        public void DumpAllDps(Type type)
        {
            var dps = GetDpFields(type);
            foreach (var fieldInfo in dps)
            {
                var dp = (DependencyProperty)fieldInfo.GetValue(null);
                Console.WriteLine("{0} {1} {2} {3}", fieldInfo.Name, dp.Name, dp.PropertyType, dp.DefaultMetadata.DefaultValue);
            }
        }

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
                var metadata = dp.DefaultMetadata;

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

        private static void DumpMetaData(IEnumerable<FieldInfo> dps)
        {
            foreach (var info in dps)
            {
                var dp = (DependencyProperty)info.GetValue(null);
                var metadata = dp.DefaultMetadata;
                var fpm = metadata as FrameworkPropertyMetadata;
                if (fpm != null)
                {
                    Console.WriteLine("{0} {1} Default: {2}, Flags: ", info.Name, metadata.GetType().Name, fpm.DefaultValue);
                }
                else
                {
                    Console.WriteLine("{0} {1} Default: {2}", info.Name, metadata.GetType().Name, metadata.DefaultValue);
                }
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
