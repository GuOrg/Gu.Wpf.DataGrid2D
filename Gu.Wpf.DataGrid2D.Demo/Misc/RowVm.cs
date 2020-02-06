namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class RowVm : List<ItemVm>, INotifyPropertyChanged
    {
        private string name;

        public RowVm(string name)
        {
            this.name = name;
        }

        public RowVm(string name, IEnumerable<ItemVm> items)
            : base(items)
        {
            this.name = name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name
        {
            get => this.name;

            set
            {
                if (value == this.name)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
