namespace Gu.Wpf.DataGrid2D
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;

    /// <devdoc>
    /// <para>Provides a type converter to convert <see cref='RowColumnIndex'/>
    /// objects to and from various
    /// other representations.</para>
    /// </devdoc>
    public class RowColumnIndexConverter : TypeConverter
    {
        /// <devdoc>
        ///    <para>Gets a value indicating whether this converter can
        ///       convert an object in the given source type to a <see cref='RowColumnIndex'/> object using the
        ///       specified context.</para>
        /// </devdoc>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <devdoc>
        ///    <para>Gets a value indicating whether this converter can
        ///       convert an object to the given destination type using the context.</para>
        /// </devdoc>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if ((destinationType == typeof(string)) || (destinationType == typeof(InstanceDescriptor)))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <devdoc>
        /// <para>Converts the given object to a <see cref='RowColumnIndex'/>
        /// object.</para>
        /// </devdoc>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var text = value as string;
            if (text != null)
            {
                RowColumnIndex result;
                if (RowColumnIndex.TryParse(text, out result))
                {
                    return result;
                }

                var message = $"Could not convert the string '{text}' to an instance of {typeof(RowColumnIndex)})";
                throw new NotSupportedException(message);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <devdoc>
        ///      Converts the given object to another type.  The most common types to convert
        ///      are to and from a string object.  The default implementation will make a call
        ///      to ToString on the object if the object is valid and if the destination
        ///      type is string.  If this cannot convert to the <paramref name="destinationType"/> type, this will
        ///      throw a NotSupportedException.
        /// </devdoc>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is RowColumnIndex)
            {
                var index = (RowColumnIndex)value;
                if (destinationType == typeof(string))
                {
                    return index.ToString();
                }

                if (destinationType == typeof(InstanceDescriptor))
                {
                    var ci = typeof(RowColumnIndex).GetConstructor(new[] { typeof(int), typeof(int) });
                    return new InstanceDescriptor(ci, new object[] { index.Row, index.Column });
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}