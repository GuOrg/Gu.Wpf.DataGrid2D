namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Gu.Wpf.DataGrid2D.Demo.Annotations;

    public class Vm : INotifyPropertyChanged
    {
        private object selectedItem;

        public Vm()
        {
            this.Data2D = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            this.ColumnHeaders = new[] { "Col1", "Col2" };
            this.JaggedRows = new int[3][];
            this.JaggedRows[0] = new[] { 1, 2 };
            this.JaggedRows[1] = new[] { 3, 4 };
            this.JaggedRows[2] = new[] { 5, 6 };

            int count = 1;
            this.ListOfListsOfItems = new List<List<ItemVm>>();
            this.RowVms = new List<RowVm>();
            this.AllRowsItems = new List<ItemVm>();
            for (int i = 0; i < 3; i++)
            {
                var row = new List<ItemVm>();
                this.ListOfListsOfItems.Add(row);

                var rowVm = new RowVm("Row" + i);
                this.RowVms.Add(rowVm);
                for (int j = 0; j < 2; j++)
                {
                    row.Add(new ItemVm(count));
                    var itemVm = new ItemVm(count);
                    rowVm.Add(itemVm);
                    this.AllRowsItems.Add(itemVm);
                    count++;
                }
            }

            this.ColumnItemHeaders = Enumerable.Range(0, 2)
                                          .Select(x => new ItemVm(x))
                                          .ToArray();
            this.RowHeaders = Enumerable.Range(0, 3).Select(x => "Row" + x).ToArray();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] ColumnHeaders { get; private set; }
        
        public string[] RowHeaders { get; private set; }
       
        public ItemVm[] ColumnItemHeaders { get; private set; }
        
        public int[][] JaggedRows { get; private set; }

        public List<List<ItemVm>> ListOfListsOfItems { get; private set; }

        public List<ItemVm> AllRowsItems { get; private set; }
        
        public List<RowVm> RowVms { get; private set; } 

        public int[,] Data2D { get; private set; }

        public object SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                if (Equals(value, this.selectedItem))
                {
                    return;
                }
                this.selectedItem = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
