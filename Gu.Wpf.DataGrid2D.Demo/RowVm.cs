using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gu.Wpf.DataGrid2D.Demo.Annotations;

namespace Gu.Wpf.DataGrid2D.Demo
{
    public class RowVm : List<ItemVm>, INotifyPropertyChanged
    {
        private string _name;

        public RowVm(string name)
        {
            _name = name;
        }

        public RowVm(string name, IEnumerable<ItemVm> items)
            :base(items)
        {
            _name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
