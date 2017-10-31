namespace Gu.Wpf.DataGrid2D.Demo
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using JetBrains.Annotations;

    public sealed class BigDataGridViewModel : INotifyPropertyChanged
    {
        public static readonly BigDataGridViewModel Default = new BigDataGridViewModel();
        private string[] rowHeaders;
        private string[] columnHeaders;
        private int[,] data2D;
        private int rows = 10; // small grid as default
        private int columns = 10;
        private TimeSpan updateTime;

        public BigDataGridViewModel()
        {
            this.UpdateDataCommand = new RelayCommand(this.UpdateData);
            this.UpdateData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] RowHeaders
        {
            get => this.rowHeaders;
            private set
            {
                if (ReferenceEquals(value, this.rowHeaders))
                {
                    return;
                }

                this.rowHeaders = value;
                this.OnPropertyChanged();
            }
        }

        public string[] ColumnHeaders
        {
            get => this.columnHeaders;
            private set
            {
                if (ReferenceEquals(value, this.columnHeaders))
                {
                    return;
                }

                this.columnHeaders = value;
                this.OnPropertyChanged();
            }
        }

        public int[,] Data2D
        {
            get => this.data2D;
            private set
            {
                if (ReferenceEquals(value, this.data2D))
                {
                    return;
                }

                this.data2D = value;
                this.OnPropertyChanged();
            }
        }

        public int Rows
        {
            get => this.rows;
            set
            {
                if (value == this.rows)
                {
                    return;
                }

                this.rows = value;
                this.OnPropertyChanged();
            }
        }

        public int Columns
        {
            get => this.columns;
            set
            {
                if (value == this.columns)
                {
                    return;
                }

                this.columns = value;
                this.OnPropertyChanged();
            }
        }

        public TimeSpan UpdateTime
        {
            get => this.updateTime;
            set
            {
                if (value.Equals(this.updateTime))
                {
                    return;
                }

                this.updateTime = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand UpdateDataCommand { get; }

        private void UpdateData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var sw = Stopwatch.StartNew();
            var data = new int[this.rows, this.columns];
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    data[i, j] = 1;
                }
            }

            this.Data2D = data;
            this.RowHeaders = Enumerable.Range(1, this.rows)
                                        .Select(r => $"R{r}")
                                        .ToArray();
            this.ColumnHeaders = Enumerable.Range(1, this.columns)
                                           .Select(r => $"C{r}")
                                           .ToArray();
            this.OnPropertyChanged(nameof(this.ColumnHeaders));
            this.OnPropertyChanged(nameof(this.RowHeaders));
            this.OnPropertyChanged(nameof(this.Data2D));
            this.UpdateTime = sw.Elapsed;
            Mouse.OverrideCursor = null;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}