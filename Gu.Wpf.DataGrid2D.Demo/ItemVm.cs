namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Gu.Wpf.DataGrid2D.Demo.Annotations;

    public class ItemVm : INotifyPropertyChanged
    {
        private int _value;

        private string _name;
        private bool _isSelected;

        public ItemVm(int value)
        {
            Value = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                OnPropertyChanged();
            }
        }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == _value)
                {
                    return;
                }
                _value = value;
                OnPropertyChanged();
                Name = "Item: " + value;
            }
        }

        public override string ToString()
        {
            return Name;
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
