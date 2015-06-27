namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.DataGrid2D.Demo.Annotations;

    public class ItemVm : INotifyPropertyChanged
    {
        public ItemVm(int value)
        {
            Value = "Item: " + value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
