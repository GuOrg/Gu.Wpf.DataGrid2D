namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using JetBrains.Annotations;

    public class JaggedVm : INotifyPropertyChanged
    {
        private string data;

        public JaggedVm()
        {
            this.RowHeaders = new[] { "1", "2", "3" };
            this.ColumnHeaders = new[] { "A", "B", "C" };

            this.SameLengths = new int[3][];
            this.SameLengths[0] = new[] { 1, 2 };
            this.SameLengths[1] = new[] { 3, 4 };
            this.SameLengths[2] = new[] { 5, 6 };

            this.DifferentLengths = new int[3][];
            this.DifferentLengths[0] = new[] { 1 };
            this.DifferentLengths[1] = new[] { 2, 3 };
            this.DifferentLengths[2] = new[] { 4, 5, 6 };
            this.UpdateDataCommand = new RelayCommand(this.UpdateData);
            this.UpdateData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public int[][] SameLengths { get; }

        public int[][] DifferentLengths { get; }

        public ICommand UpdateDataCommand { get; }

        public string Data
        {
            get
            {
                return this.data;
            }

            private set
            {
                if (value == this.data)
                {
                    return;
                }

                this.data = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateData()
        {
            this.Data = $"{{{string.Join(", ", this.SameLengths.Select(x => $"{{{string.Join(", ", x)}}}"))}}}";
        }
    }
}
