namespace Gu.Wpf.DataGrid2D.Demo
{
    using JetBrains.Annotations;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public class DemoClass
    {
        public int Value1 { get; set; }

        public int Value2 { get; set; }

        public SolidColorBrush Background { get; set; }
    }

    public class CellTemplateVm : INotifyPropertyChanged
    {
        private string data;
        private int currenttemplate = 3;

        public CellTemplateVm()
        {
            this.RowHeaders = new[] { "1", "2", "3" };
            this.ColumnHeaders = new[] { "A", "B", "C" };

            this.Data2D = new DemoClass[3, 3];
            Random r = new Random();
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    DemoClass cl = new DemoClass();
                    cl.Value1 = i + j;
                    cl.Value2 = 9 - j - i;

                    cl.Background = new SolidColorBrush(new Color()
                    {
                        A = (byte)r.Next(255),
                        R = (byte)r.Next(255),
                        G = (byte)r.Next(255),
                        B = (byte)r.Next(255)
                    });
                    this.Data2D[i, j] = cl;
                }
            }

            this.UpdateDataCommand = new RelayCommand(this.UpdateData);

            this.UpdateData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public ICommand UpdateDataCommand { get; }

        public ICommand ChangeCellTemplateCommand { get; }

        public DemoClass[,] Data2D { get; }

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
            // ? todo
        }
    }
}
