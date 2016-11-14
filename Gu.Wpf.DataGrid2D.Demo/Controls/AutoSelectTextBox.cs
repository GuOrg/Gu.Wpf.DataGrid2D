namespace Gu.Wpf.DataGrid2D.Demo.Controls
{
    using System;
    using System.Windows.Controls;
    using JetBrains.Annotations;

    public class AutoSelectTextBox : TextBox
    {
        private bool autoSelectAll = true;

        protected override void OnInitialized(EventArgs e)
        {
            // This will cause the cursor to enter the text box ready to
            // type even when there is no content.
            this.Focus();
            base.OnInitialized(e);
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            // This is here to handle the case of an empty text box.  If
            // omitted then the first character would be auto selected when
            // the user starts typing.
            this.autoSelectAll = false;
            base.OnKeyDown(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (this.autoSelectAll)
            {
                this.SelectAll();
                this.Focus();
                this.autoSelectAll = false;
            }

            base.OnTextChanged(e);
        }
    }
}
