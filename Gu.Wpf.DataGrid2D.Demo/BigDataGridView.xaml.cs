namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Windows.Controls;

    public partial class BigDataGridView : UserControl
    {
        public BigDataGridView()
        {
            this.InitializeComponent();
        }
    }

    public class BigDataGridViewModel
    {
        public static readonly BigDataGridViewModel Default = new BigDataGridViewModel();
    }
}
