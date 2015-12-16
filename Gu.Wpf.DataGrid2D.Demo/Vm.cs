namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Gu.Wpf.DataGrid2D.Demo.Annotations;

    public class Vm : INotifyPropertyChanged
    {
        public Vm()
        {



            this.RowHeaders = Enumerable.Range(0, 3).Select(x => "Row" + x).ToArray();
        }

        public event PropertyChangedEventHandler PropertyChanged;

       
        public string[] RowHeaders { get; private set; }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
