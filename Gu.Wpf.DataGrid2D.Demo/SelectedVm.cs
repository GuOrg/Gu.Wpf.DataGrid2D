namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class SelectedVm : INotifyPropertyChanged
    {
        private ItemVm selectedItem;
        private RowColumnIndex? index;

        public SelectedVm()
        {
            var rowVms = new List<RowVm>();
            int count = 1;

            for (int i = 0; i < 3; i++)
            {
                var rowVm = new RowVm("Row" + i);
                rowVms.Add(rowVm);
                for (int j = 0; j < 2; j++)
                {
                    var itemVm = new ItemVm(count);
                    rowVm.Add(itemVm);
                    count++;
                }
            }

            this.RowVms = rowVms;
            this.AllRowsItems = rowVms.SelectMany(x=>x).ToList();
            this.Indices = new[]
                           {
                               new RowColumnIndex(0, 0),
                               new RowColumnIndex(1, 0),
                               new RowColumnIndex(2, 0),
                               new RowColumnIndex(0, 1),
                               new RowColumnIndex(1, 1),
                               new RowColumnIndex(2, 1),
                           };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyList<RowVm> RowVms { get; }

        public IReadOnlyList<ItemVm> AllRowsItems { get; }
        public IReadOnlyList<RowColumnIndex> Indices { get; }

        public ItemVm SelectedItem
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

        public RowColumnIndex? Index
        {
            get { return this.index; }
            set
            {
                if (value.Equals(this.index)) return;
                this.index = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
