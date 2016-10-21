namespace Gu.Wpf.DataGrid2D.Demo
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using JetBrains.Annotations;

    public class CellTemplateDemoClass : INotifyPropertyChanged
    {
        private int value1;
        private int value2;
        private SolidColorBrush background;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Value1
        {
            get
            {
                return this.value1;
            }

            set
            {
                if (value == this.value1)
                {
                    return;
                }

                this.value1 = value;
                this.OnPropertyChanged();
            }
        }

        public int Value2
        {
            get
            {
                return this.value2;
            }

            set
            {
                if (value == this.value2)
                {
                    return;
                }

                this.value2 = value;
                this.OnPropertyChanged();
            }
        }

        public SolidColorBrush Background
        {
            get
            {
                return this.background;
            }

            set
            {
                if (Equals(value, this.background))
                {
                    return;
                }

                this.background = value;
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
