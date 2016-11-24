namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.ObjectModel;

    public class SingleClickEditViewModel
    {
        public SingleClickEditViewModel()
        {
            this.Items = new ObservableCollection<SingleClickEditItem>
                    {
                        new SingleClickEditItem(),
                        new SingleClickEditItem(),
                    };
        }

        public ObservableCollection<SingleClickEditItem> Items { get;  }
    }
}
