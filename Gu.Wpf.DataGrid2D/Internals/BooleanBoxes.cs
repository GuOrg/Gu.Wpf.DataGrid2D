namespace Gu.Wpf.DataGrid2D
{
    internal static class BooleanBoxes
    {
        internal static object True = true;
        internal static object False = false;

        internal static object Box(bool value)
        {
            if (value)
            {
                return True;
            }
            else
            {
                return False;
            }
        }
    }
}