using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gu.Wpf.DataGrid2D.Internals
{
    internal class CustomDataGridTemplateColumn : DataGridTemplateColumn
    {
        private BindingBase _binding;

        protected virtual void OnBindingChanged(BindingBase oldBinding, BindingBase newBinding)
        {
            base.NotifyPropertyChanged("Binding");
        }

        public virtual BindingBase Binding
        {
            get
            {
                return this._binding;
            }
            set
            {
                if (this._binding != value)
                {
                    BindingBase oldBinding = this._binding;
                    this._binding = value;
                    base.CoerceValue(DataGridColumn.SortMemberPathProperty);
                    this.OnBindingChanged(oldBinding, this._binding);
                }
            }
        }

        public override BindingBase ClipboardContentBinding
        {
            get
            {
                return (base.ClipboardContentBinding ?? this.Binding);
            }
            set
            {
                base.ClipboardContentBinding = value;
            }
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
