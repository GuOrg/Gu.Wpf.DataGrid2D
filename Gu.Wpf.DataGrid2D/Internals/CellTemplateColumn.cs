namespace Gu.Wpf.DataGrid2D
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Gu.Wpf.DataGrid2D.Internals;

    internal class CellTemplateColumn : DataGridTemplateColumn
    {
        private BindingBase binding;

        public BindingBase Binding
        {
            get => this.binding;

            set
            {
                if (this.binding != value)
                {
                    this.binding = value;
                    this.CoerceValue(SortMemberPathProperty);
                    this.NotifyPropertyChanged(nameof(this.Binding));
                }
            }
        }

        public override BindingBase ClipboardContentBinding
        {
            get => base.ClipboardContentBinding ?? this.Binding;
            set => base.ClipboardContentBinding = value;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return this.LoadTemplateContent(isEditing: true, dataItem: dataItem, cell: cell);
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return this.LoadTemplateContent(isEditing: false, dataItem: dataItem, cell: cell);
        }

        private DataTemplate ChooseCellTemplate(bool isEditing)
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

        private DataTemplateSelector ChooseCellTemplateSelector(bool isEditing)
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

        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private FrameworkElement LoadTemplateContent(bool isEditing, object dataItem, DataGridCell cell)
        {
            var template = this.ChooseCellTemplate(isEditing);
            var templateSelector = this.ChooseCellTemplateSelector(isEditing);
            if ((template == null) && (templateSelector == null))
            {
                return null;
            }

            var contentPresenter = new ContentPresenter();
            BindingOperations.SetBinding(contentPresenter, ContentPresenter.ContentProperty, this.binding)
                             .IgnoreReturnValue();
            contentPresenter.ContentTemplate = template;
            contentPresenter.ContentTemplateSelector = templateSelector;
            return contentPresenter;
        }
    }
}
