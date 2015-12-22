namespace Gu.Wpf.DataGrid2D.Tests
{
    using System.ComponentModel.Design.Serialization;
    using NUnit.Framework;

    public class RowColumnIndexConverterTests
    {
        [Test]
        public void StringRoundtrips()
        {
            var index = new RowColumnIndex(1, 2);
            var converter = new RowColumnIndexConverter();

            Assert.IsTrue(converter.CanConvertTo(null, typeof(string)));
            var convertTo = converter.ConvertTo(index, typeof(string));
            Assert.AreEqual("R1 C2", convertTo);

            Assert.IsTrue(converter.CanConvertFrom(null, typeof(string)));

            var convertFrom = converter.ConvertFrom(convertTo);
            Assert.AreEqual(index, convertFrom);
        }

        [Test]
        public void InstanceDescriptorRoundtrip()
        {
            var index = new RowColumnIndex(1, 2);
            var converter = new RowColumnIndexConverter();

            Assert.IsTrue(converter.CanConvertTo(null, typeof(InstanceDescriptor)));
            var convertTo = converter.ConvertTo(index, typeof(InstanceDescriptor));
            Assert.IsInstanceOf<InstanceDescriptor>(convertTo);

            Assert.IsTrue(converter.CanConvertFrom(null, typeof(InstanceDescriptor)));

            var convertFrom = converter.ConvertFrom(convertTo);
            Assert.AreEqual(index, convertFrom);
        }
    }
}