namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class SingleClickEditItem : INotifyPropertyChanged
    {
        private string stringValue;
        private int intValue;
        private bool boolValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public string StringValue
        {
            get
            {
                return this.stringValue;
            }

            set
            {
                if (value == this.stringValue)
                {
                    return;
                }
                this.stringValue = value;
                this.OnPropertyChanged();
            }
        }

        public int IntValue
        {
            get
            {
                return this.intValue;
            }

            set
            {
                if (value == this.intValue)
                {
                    return;
                }
                this.intValue = value;
                this.OnPropertyChanged();
            }
        }

        public bool BoolValue
        {
            get
            {
                return this.boolValue;
            }

            set
            {
                if (value == this.boolValue)
                {
                    return;
                }
                this.boolValue = value;
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