namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class Vm : INotifyPropertyChanged
    {
        public Vm()
        {
            this.RowHeaders = Enumerable.Range(0, 3).Select(x => "Row" + x).ToArray();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string[] RowHeaders { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
