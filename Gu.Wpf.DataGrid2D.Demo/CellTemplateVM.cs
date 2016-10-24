namespace Gu.Wpf.DataGrid2D.Demo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using System.Windows.Media;
    using JetBrains.Annotations;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class CellTemplateVm : INotifyPropertyChanged
    {
        private string data;
        private DataTemplate dt1;
        private DataTemplate dt2;

        public CellTemplateVm()
        {
            this.RowHeaders = new[] { "1", "2", "3" };
            this.ColumnHeaders = new[] { "A", "B", "C" };

            this.dt1 = this.CreateDataTemplate("Value1");
            this.dt2 = this.CreateDataTemplate("Value1");

            this.MyCellTemplate = this.dt1;

            this.Data2D = new CellTemplateDemoClass[3, 3];
            Random r = new Random();
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    CellTemplateDemoClass cl = new CellTemplateDemoClass();
                    cl.Value1 = i + j;
                    cl.Value2 = 9 - j - i;

                    cl.Background = new SolidColorBrush(new Color()
                    {
                        A = (byte)r.Next(255),
                        R = (byte)r.Next(255),
                        G = (byte)r.Next(255),
                        B = (byte)r.Next(255)
                    });
                    this.Data2D[i, j] = cl;
                }
            }

            this.UpdateDataCommand = new RelayCommand(this.UpdateData);
            this.ChangeCellTemplateCommand = new RelayCommand(this.ChangeCellTemplate);

            this.UpdateData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public ICommand UpdateDataCommand { get; }

        public ICommand ChangeCellTemplateCommand { get; }

        public DataTemplate MyCellTemplate { get; set; }

        public CellTemplateDemoClass[,] Data2D { get; }

        public string BoundTemplate
        {
            get
            {
                if (this.MyCellTemplate == this.dt1)
                {
                    return "CellTemplate with binding to Value1";
                }
                else if (this.MyCellTemplate == this.dt2)
                {
                    return "CellTemplate with binding to Value2";
                }
                else
                {
                    return "CellTemplate set to null";
                }
            }
        }

        public string Data
        {
            get
            {
                return this.data;
            }

            private set
            {
                if (value == this.data)
                {
                    return;
                }

                this.data = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangeCellTemplate()
        {
            if (this.MyCellTemplate == this.dt1)
            {
                this.MyCellTemplate = this.dt2;
            }
            else if (this.MyCellTemplate == this.dt2)
            {
                this.MyCellTemplate = null;
            }
            else
            {
                this.MyCellTemplate = this.dt1;
            }

            this.OnPropertyChanged("MyCellTemplate");
            this.OnPropertyChanged("BoundTemplate");
        }

        private DataTemplate CreateDataTemplate(string property)
        {
            var dt = new DataTemplate();
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            var title = new FrameworkElementFactory(typeof(TextBlock));
            title.SetBinding(TextBlock.TextProperty, new Binding(property));
            stackPanelFactory.AppendChild(title);
            dt.VisualTree = stackPanelFactory;
            return dt;
        }

        private void UpdateData()
        {
            // ? todo
        }
    }
}
