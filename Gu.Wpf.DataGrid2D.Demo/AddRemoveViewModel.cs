namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class AddRemoveViewModel
    {
        private int columns;

        public AddRemoveViewModel()
        {
            this.Data = new ObservableCollection<ObservableCollection<int>>();

            this.AddRowCommand = new RelayCommand(this.AddRow);
            this.AddColumnCommand = new RelayCommand(this.AddColumn);
            this.RemoveLastRowCommand = new RelayCommand(this.RemoveRow);
            this.RemoveLastColumnCommand = new RelayCommand(this.RemoveColumn);

            this.columns = 0;
        }

        public ICommand AddRowCommand { get; }

        public ICommand AddColumnCommand { get; }

        public ICommand RemoveLastColumnCommand { get; }

        public ICommand RemoveLastRowCommand { get; }

        public ObservableCollection<ObservableCollection<int>> Data { get; }

        private void AddRow()
        {
            var newRow = Enumerable.Range(0, this.columns).ToArray();
            this.Data.Add(new ObservableCollection<int>(newRow));
        }

        private void AddColumn()
        {
            foreach (var row in this.Data)
            {
                row.Add(this.columns);
            }

            this.columns++;
        }

        private void RemoveColumn()
        {
            if (this.columns > 0)
            {
                foreach (var row in this.Data)
                {
                    row.RemoveAt(this.columns - 1);
                }

                this.columns--;
            }
        }

        private void RemoveRow()
        {
            var index = this.Data.Count - 1;
            if (index >= 0)
            {
                this.Data.RemoveAt(index);
            }
        }
    }
}
