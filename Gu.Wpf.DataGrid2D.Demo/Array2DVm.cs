namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class Array2DVm : INotifyPropertyChanged
    {
        private string? data;

        public Array2DVm()
        {
            this.Data2D = new[,]
                          {
                              { 1, 2 },
                              { 3, 4 },
                              { 5, 6 },
                          };
            this.RowHeaders = new[] { "1", "2", "3" };
            this.ColumnHeaders = new[] { "A", "B", "C" };
            this.UpdateDataCommand = new RelayCommand(this.UpdateData);
            this.UpdateData();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string[] RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public int[,] Data2D { get; }

        public ICommand UpdateDataCommand { get; }

        public string? Data
        {
            get => this.data;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static IEnumerable<IEnumerable<T>> Rows<T>(T[,] source)
        {
            for (int r = 0; r < source.GetLength(0); r++)
            {
                var row = new List<T>();
                for (int c = 0; c < source.GetLength(1); c++)
                {
                    row.Add(source[r, c]);
                }

                yield return row;
            }
        }

        private void UpdateData()
        {
            this.Data = $"{{{string.Join(", ", Rows(this.Data2D).Select(x => $"{{{string.Join(", ", x)}}}"))}}}";
        }
    }
}
