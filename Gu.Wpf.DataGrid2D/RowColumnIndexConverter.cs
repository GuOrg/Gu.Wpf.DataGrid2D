namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;

    /// <inheritdoc />
    public class RowColumnIndexConverter : TypeConverter
    {
        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if ((destinationType == typeof(string)) || (destinationType == typeof(InstanceDescriptor)))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string text)
            {
                if (RowColumnIndex.TryParse(text, out var result))
                {
                    return result;
                }

                var message = $"Could not convert the string '{text}' to an instance of {typeof(RowColumnIndex)})";
                throw new NotSupportedException(message);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is RowColumnIndex index)
            {
                if (destinationType == typeof(string))
                {
                    return index.ToString();
                }

                if (destinationType == typeof(InstanceDescriptor))
                {
                    var ci = typeof(RowColumnIndex).GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(int), typeof(int) }, null);
                    return new InstanceDescriptor(ci, new object[] { index.Row, index.Column });
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
