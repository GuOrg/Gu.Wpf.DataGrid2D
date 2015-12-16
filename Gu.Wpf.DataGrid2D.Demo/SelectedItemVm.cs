namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Gu.Wpf.DataGrid2D.Demo.Annotations;

    public class SelectedItemVm : INotifyPropertyChanged
    {
        private object selectedItem;

        public SelectedItemVm()
        {
            var rowVms = new List<RowVm>();
            var allRowsItems = new List<ItemVm>();
            int count = 0;

            for (int i = 0; i < 3; i++)
            {
                var rowVm = new RowVm("Row" + i);
                rowVms.Add(rowVm);
                for (int j = 0; j < 2; j++)
                {
                    var itemVm = new ItemVm(count);
                    rowVm.Add(itemVm);
                    allRowsItems.Add(itemVm);
                    count++;
                }
            }

            this.RowVms = rowVms;
            this.AllRowsItems = allRowsItems;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyList<RowVm> RowVms { get; }

        public IReadOnlyList<ItemVm> AllRowsItems { get; }

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
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
