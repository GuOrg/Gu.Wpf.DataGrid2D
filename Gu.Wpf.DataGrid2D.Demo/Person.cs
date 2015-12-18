namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Gu.Wpf.DataGrid2D.Demo.Annotations;

    public class Person : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;
        public event PropertyChangedEventHandler PropertyChanged;

        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                if (value == this.firstName) return;
                this.firstName = value;
                this.OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return this.lastName; }
            set
            {
                if (value == this.lastName) return;
                this.lastName = value;
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