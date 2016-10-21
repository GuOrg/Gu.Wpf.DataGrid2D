namespace Gu.Wpf.DataGrid2D.Demo
{
    using JetBrains.Annotations;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class DemoClass
    {
        public string Value1 { get; set; }

        public string Value2 { get; set; }
    }

    public class CellTemplateVm : INotifyPropertyChanged
    {
        private string data;

        public CellTemplateVm()
        {
            this.RowHeaders = new[] { "1", "2", "3" };
            this.ColumnHeaders = new[] { "A", "B", "C" };

            this.Data2D = new DemoClass[3, 3];
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    DemoClass cl = new DemoClass();
                    cl.Value1 = i.ToString() + j.ToString();
                    cl.Value2 = j.ToString() + i.ToString();
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

        }
    }
}
