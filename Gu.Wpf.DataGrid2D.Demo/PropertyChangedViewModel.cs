namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class PropertyChangedViewModel : INotifyPropertyChanged
    {
        private string[,] strings =
        {
            { "1", "2" },
            { "3", "4" },
            { "5", "6" },
        };

        private int count;

        public PropertyChangedViewModel()
        {
            this.UpdateDataCommand = new RelayCommand(this.UpdateData);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string[,] Strings
        {
            get => this.strings;

            set
            {
                if (Equals(value, this.strings))
                {
                    return;
                }

                this.strings = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand UpdateDataCommand { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateData()
        {
            this.count++;
            this.Strings = new[,]
                           {
                               { (this.count + 1).ToString(CultureInfo.InvariantCulture), (this.count + 2).ToString(CultureInfo.InvariantCulture) },
                               { (this.count + 3).ToString(CultureInfo.InvariantCulture), (this.count + 4).ToString(CultureInfo.InvariantCulture) },
                               { (this.count + 5).ToString(CultureInfo.InvariantCulture), (this.count + 6).ToString(CultureInfo.InvariantCulture) },
                           };
        }
    }
}
