namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.ObjectModel;

    public class ThreeStateSortViewModel
    {
        public ThreeStateSortViewModel()
        {
            this.Items = new ObservableCollection<ThreeStateSortItem>
                         {
                             new ThreeStateSortItem {StringValue = "a", IntValue = 1},
                             new ThreeStateSortItem {StringValue = "c", IntValue = 4},
                             new ThreeStateSortItem {StringValue = "d", IntValue = 2},
                             new ThreeStateSortItem {StringValue = "b", IntValue = 3},
                         };
        }

        public ObservableCollection<ThreeStateSortItem> Items { get;  }
    }
}