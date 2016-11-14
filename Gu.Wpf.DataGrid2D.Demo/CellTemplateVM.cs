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
    using JetBrains.Annotations;

    public class CellTemplateVm : INotifyPropertyChanged
    {
        private string data;
        private DataTemplate celltemplate1;
        private DataTemplate celltemplate2;
        private DataTemplate celleditingtemplate1;
        private DataTemplate celleditingtemplate2;

        public CellTemplateVm()
        {
            this.RowHeaders = new[] { "1", "2", "3" };
            this.ColumnHeaders = new[] { "C1", "C2", "C3" };

            this.celltemplate1 = this.CreateCellTemplate("Value1");
            this.celltemplate2 = this.CreateCellTemplate("Value2");
            this.celleditingtemplate1 = this.CreateCellEditingTemplate("Value1");
            this.celleditingtemplate2 = this.CreateCellEditingTemplate("Value2");

            this.MyCellTemplate = this.celltemplate1;
            this.MyCellEditingTemplate = this.celleditingtemplate1;

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

            this.ChangeCellTemplateCommand = new RelayCommand(this.ChangeCellTemplate);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] RowHeaders { get; }

        public string[] ColumnHeaders { get; }

        public ICommand UpdateDataCommand { get; }

        public ICommand ChangeCellTemplateCommand { get; }

        public DataTemplate MyCellTemplate { get; set; }

        public DataTemplate MyCellEditingTemplate { get; set; }

        public CellTemplateDemoClass[,] Data2D { get; }

        public string BoundTemplate
        {
            get
            {
                if (this.MyCellTemplate == this.celltemplate1 && this.MyCellEditingTemplate == this.celleditingtemplate1)
                {
                    return "CellTemplate and CellEditingTemplate with binding to Value1";
                }
                else if (this.MyCellTemplate == this.celltemplate2 && this.MyCellEditingTemplate == this.celleditingtemplate2)
                {
                    return "CellTemplate and CellEditingTemplate with binding to Value2";
                }
                else if (this.MyCellTemplate == this.celltemplate1 && this.MyCellEditingTemplate == null)
                {
                    return "CellTemplate with binding to Value1, CellEditingTemplate set to null";
                }
                else if (this.MyCellTemplate == null && this.MyCellEditingTemplate == this.celleditingtemplate1)
                {
                    return "CellTemplate set to null, CellEditingTemplate with binding to Value1";
                }
                else
                {
                    return "CellTemplate and CellEditingTemplate both set to null";
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
            if (this.MyCellTemplate == this.celltemplate1 && this.MyCellEditingTemplate == this.celleditingtemplate1)
            {
                this.MyCellTemplate = this.celltemplate2;
                this.MyCellEditingTemplate = this.celleditingtemplate2;
            }
            else if (this.MyCellTemplate == this.celltemplate2 && this.MyCellEditingTemplate == this.celleditingtemplate2)
            {
                this.MyCellTemplate = this.celltemplate1;
                this.MyCellEditingTemplate = null;
            }
            else if (this.MyCellTemplate == this.celltemplate1 && this.MyCellEditingTemplate == null)
            {
                this.MyCellTemplate = null;
                this.MyCellEditingTemplate = this.celleditingtemplate1;
            }
            else if (this.MyCellTemplate == null && this.MyCellEditingTemplate == this.celleditingtemplate1)
            {
                this.MyCellTemplate = null;
                this.MyCellEditingTemplate = null;
            }
            else
            {
                this.MyCellTemplate = this.celltemplate1;
                this.MyCellEditingTemplate = this.celleditingtemplate1;
            }

            this.OnPropertyChanged("MyCellTemplate");
            this.OnPropertyChanged("MyCellEditingTemplate");
            this.OnPropertyChanged("BoundTemplate");
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
