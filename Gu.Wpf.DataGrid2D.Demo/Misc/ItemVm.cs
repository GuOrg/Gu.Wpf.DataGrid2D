namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ItemVm : INotifyPropertyChanged
    {
        private int value;

        private string? name;
        private bool isSelected;

        public ItemVm(int value)
        {
            this.Value = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsSelected
        {
            get => this.isSelected;

            set
            {
                if (value == this.isSelected)
                {
                    return;
                }

                this.isSelected = value;
                this.OnPropertyChanged();
            }
        }

        public string? Name
        {
            get => this.name;

            set
            {
                if (value == this.name)
                {
                    return;
                }

                this.name = value;
                this.OnPropertyChanged();
            }
        }

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
                this.Name = "Item: " + value;
            }
        }

        public override string? ToString()
        {
            return this.Name;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
