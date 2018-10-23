namespace Gu.Wpf.DataGrid2D.Demo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    public class CellTemplateVm : INotifyPropertyChanged
    {
        private readonly DataTemplate cellTemplate1;
        private readonly DataTemplate cellTemplate2;
        private readonly DataTemplate cellEditingTemplate1;
        private readonly DataTemplate cellEditingTemplate2;
        private DataTemplate myCellTemplate;
        private DataTemplate myCellEditingTemplate;

        public CellTemplateVm()
        {
            this.RowHeaders = new[] { "R1", "R2", "R3" };
            this.ColumnHeaders = new[] { "C1", "C2", "C3" };

            this.cellTemplate1 = this.CreateCellTemplate("Value1");
            this.cellTemplate2 = this.CreateCellTemplate("Value2");
            this.cellEditingTemplate1 = this.CreateCellEditingTemplate("Value1");
            this.cellEditingTemplate2 = this.CreateCellEditingTemplate("Value2");

            this.myCellTemplate = this.cellTemplate1;
            this.myCellEditingTemplate = this.cellEditingTemplate1;

            this.Data2D = new CellTemplateDemoClass[3, 3];
            var r = new Random();
            for (var i = 0; i < 3; ++i)
            {
                for (var j = 0; j < 3; ++j)
                {
                    var cl = new CellTemplateDemoClass
                    {
                        Value1 = i + j,
                        Value2 = 9 - j - i,
                        Background = new SolidColorBrush(new Color
                        {
                            A = (byte)r.Next(255),
                            R = (byte)r.Next(255),
                            G = (byte)r.Next(255),
                            B = (byte)r.Next(255),
                        }),
                    };

                    this.Data2D[i, j] = cl;
                }
            }

            this.ChangeCellTemplateCommand = new RelayCommand(this.ChangeCellTemplate);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public ICommand UpdateDataCommand { get; }

        public ICommand ChangeCellTemplateCommand { get; }

        public CellTemplateDemoClass[,] Data2D { get; }

        public string BoundTemplate
        {
            get
            {
                if (this.MyCellTemplate == this.cellTemplate1 && this.MyCellEditingTemplate == this.cellEditingTemplate1)
                {
                    return "CellTemplate and CellEditingTemplate with binding to Value1";
                }
                else if (this.MyCellTemplate == this.cellTemplate2 && this.MyCellEditingTemplate == this.cellEditingTemplate2)
                {
                    return "CellTemplate and CellEditingTemplate with binding to Value2";
                }
                else if (this.MyCellTemplate == this.cellTemplate1 && this.MyCellEditingTemplate == null)
                {
                    return "CellTemplate with binding to Value1, CellEditingTemplate set to null";
                }
                else if (this.MyCellTemplate == null && this.MyCellEditingTemplate == this.cellEditingTemplate1)
                {
                    return "CellTemplate set to null, CellEditingTemplate with binding to Value1";
                }
                else
                {
                    return "CellTemplate and CellEditingTemplate both set to null";
                }
            }
        }

        public DataTemplate MyCellTemplate
        {
            get => this.myCellTemplate;
            set
            {
                if (ReferenceEquals(value, this.myCellTemplate))
                {
                    return;
                }

                this.myCellTemplate = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.BoundTemplate));
            }
        }

        public DataTemplate MyCellEditingTemplate
        {
            get => this.myCellEditingTemplate;
            set
            {
                if (ReferenceEquals(value, this.myCellEditingTemplate))
                {
                    return;
                }

                this.myCellEditingTemplate = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(this.BoundTemplate));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChangeCellTemplate()
        {
            if (this.MyCellTemplate == this.cellTemplate1 && this.MyCellEditingTemplate == this.cellEditingTemplate1)
            {
                this.MyCellTemplate = this.cellTemplate2;
                this.MyCellEditingTemplate = this.cellEditingTemplate2;
            }
            else if (this.MyCellTemplate == this.cellTemplate2 && this.MyCellEditingTemplate == this.cellEditingTemplate2)
            {
                this.MyCellTemplate = this.cellTemplate1;
                this.MyCellEditingTemplate = null;
            }
            else if (this.MyCellTemplate == this.cellTemplate1 && this.MyCellEditingTemplate == null)
            {
                this.MyCellTemplate = null;
                this.MyCellEditingTemplate = this.cellEditingTemplate1;
            }
            else if (this.MyCellTemplate == null && this.MyCellEditingTemplate == this.cellEditingTemplate1)
            {
                this.MyCellTemplate = null;
                this.MyCellEditingTemplate = null;
            }
            else
            {
                this.MyCellTemplate = this.cellTemplate1;
                this.MyCellEditingTemplate = this.cellEditingTemplate1;
            }

            this.OnPropertyChanged(nameof(this.MyCellTemplate));
            this.OnPropertyChanged(nameof(this.MyCellEditingTemplate));
            this.OnPropertyChanged(nameof(this.BoundTemplate));
        }

        private DataTemplate CreateCellTemplate(string property)
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

        private DataTemplate CreateCellEditingTemplate(string property)
        {
            var dt = new DataTemplate();
            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            var title = new FrameworkElementFactory(typeof(TextBox));
            title.SetBinding(TextBox.TextProperty, new Binding(property));
            stackPanelFactory.AppendChild(title);
            dt.VisualTree = stackPanelFactory;
            return dt;
        }
    }
}
