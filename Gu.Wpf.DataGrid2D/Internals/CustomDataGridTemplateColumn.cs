namespace Gu.Wpf.DataGrid2D
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class CustomDataGridTemplateColumn : DataGridTemplateColumn
    {
        private BindingBase binding;

        public virtual BindingBase Binding
        {
            get
            {
                return this.binding;
            }

            set
            {
                if (this.binding != value)
                {
                    BindingBase oldBinding = this.binding;
                    this.binding = value;
                    this.CoerceValue(DataGridColumn.SortMemberPathProperty);
                    this.OnBindingChanged(oldBinding, this.binding);
                }
            }
        }

        public override BindingBase ClipboardContentBinding
        {
            get { return base.ClipboardContentBinding ?? this.Binding; }
            set { base.ClipboardContentBinding = value; }
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        protected virtual void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
        {
            this.NotifyPropertyChanged("Binding");
        }

        protected virtual DataTemplate ChooseCellTemplate(bool isEditing)
        {
            DataTemplate template = null;
            if (isEditing)
            {
                template = this.CellEditingTemplate;
            }

            if (template == null)
            {
                template = this.CellTemplate;
            }

            return template;
        }

        protected virtual DataTemplateSelector ChooseCellTemplateSelector(bool isEditing)
        {
            DataTemplateSelector templateSelector = null;
            if (isEditing)
            {
                templateSelector = this.CellEditingTemplateSelector;
            }

            if (templateSelector == null)
            {
                templateSelector = this.CellTemplateSelector;
            }

            return templateSelector;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return this.LoadTemplateContent(true, dataItem, cell);
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return this.LoadTemplateContent(false, dataItem, cell);
        }

        private void ApplyBinding(DependencyObject target, DependencyProperty property)
        {
            BindingBase binding = this.Binding;
            if (binding != null)
            {
                BindingOperations.SetBinding(target, property, binding);
            }
            else
            {
                BindingOperations.SetBinding(target, property, new Binding());
            }
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private FrameworkElement LoadTemplateContent(bool isEditing, object dataItem, DataGridCell cell)
        {
            DataTemplate template = this.ChooseCellTemplate(isEditing);
            DataTemplateSelector templateSelector = this.ChooseCellTemplateSelector(isEditing);
            if ((template == null) && (templateSelector == null))
            {
                return null;
            }

            ContentPresenter contentPresenter = new ContentPresenter();
            this.ApplyBinding(contentPresenter, ContentPresenter.ContentProperty);
            contentPresenter.ContentTemplate = template;
            contentPresenter.ContentTemplateSelector = templateSelector;
            return contentPresenter;
        }
    }
}
