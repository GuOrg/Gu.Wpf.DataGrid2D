namespace Gu.Wpf.DataGrid2D.Tests.Internals
{
    public class DummyItem : System.ComponentModel.INotifyPropertyChanged
    {
        private int value;

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        public int Value
        {
            get => this.value;
            set
            {
                if (value == this.value)
                {
                    return;
                }

                this.value = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}