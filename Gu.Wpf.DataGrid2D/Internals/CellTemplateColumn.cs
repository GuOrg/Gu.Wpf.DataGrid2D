namespace Gu.Wpf.DataGrid2D
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class CellTemplateColumn : DataGridTemplateColumn
    {
        private BindingBase? binding;

        /// <inheritdoc/>
        public override BindingBase? ClipboardContentBinding
        {
            get => base.ClipboardContentBinding ?? this.Binding;
            set => base.ClipboardContentBinding = value;
        }

        internal BindingBase? Binding
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

        /// <inheritdoc/>
        protected override FrameworkElement? GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return this.LoadTemplateContent(isEditing: true);
        }

        /// <inheritdoc/>
        protected override FrameworkElement? GenerateElement(DataGridCell cell, object dataItem)
        {
            return this.LoadTemplateContent(isEditing: false);
        }

        private DataTemplate ChooseCellTemplate(bool isEditing)
        {
            if (isEditing)
            {
               return this.CellEditingTemplate ?? this.CellTemplate;
            }

            return this.CellTemplate;
        }

        private DataTemplateSelector ChooseCellTemplateSelector(bool isEditing)
        {
            if (isEditing)
            {
                return this.CellEditingTemplateSelector ?? this.CellTemplateSelector;
            }

            return this.CellTemplateSelector;
        }

        private FrameworkElement? LoadTemplateContent(bool isEditing)
        {
            var template = this.ChooseCellTemplate(isEditing);
            var templateSelector = this.ChooseCellTemplateSelector(isEditing);
            if (template is null && templateSelector is null)
            {
                return null;
            }

            var contentPresenter = new ContentPresenter();
            _ = BindingOperations.SetBinding(contentPresenter, ContentPresenter.ContentProperty, this.binding);
            contentPresenter.ContentTemplate = template;
            contentPresenter.ContentTemplateSelector = templateSelector;
            return contentPresenter;
        }
    }
}
