namespace Gu.Wpf.DataGrid2D.Demo;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public class JaggedVm : INotifyPropertyChanged
{
    private IReadOnlyList<string>? currentRowHeaders;
    private IReadOnlyList<string>? currentColumnHeaders;
    private string? data;

    public JaggedVm()
    {
        this.RowHeaders = new[] { "1", "2", "3" };
        this.currentRowHeaders = this.RowHeaders;
        this.ColumnHeaders = new[] { "A", "B", "C" };
        this.currentColumnHeaders = this.ColumnHeaders;

        this.SameLengths = new int[3][];
        this.SameLengths[0] = new[] { 1, 2 };
        this.SameLengths[1] = new[] { 3, 4 };
        this.SameLengths[2] = new[] { 5, 6 };

        this.DifferentLengths = new int[3][];
        this.DifferentLengths[0] = new[] { 1 };
        this.DifferentLengths[1] = new[] { 2, 3 };
        this.DifferentLengths[2] = new[] { 4, 5, 6 };
        this.UpdateDataCommand = new RelayCommand(this.UpdateData);
        this.UseRowHeadersCommand = new RelayCommand(() => this.CurrentRowHeaders = this.RowHeaders);
        this.NullRowHeadersCommand = new RelayCommand(() => this.CurrentRowHeaders = null);
        this.UseColumnHeadersCommand = new RelayCommand(() => this.CurrentColumnHeaders = this.ColumnHeaders);
        this.NullColumnHeadersCommand = new RelayCommand(() => this.CurrentColumnHeaders = null);
        this.UpdateData();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public IReadOnlyList<string> RowHeaders { get; }

    public IReadOnlyList<string> ColumnHeaders { get; }

    public int[][] SameLengths { get; }

    public int[][] DifferentLengths { get; }

    public ICommand UpdateDataCommand { get; }

    public ICommand UseRowHeadersCommand { get; }

    public ICommand NullRowHeadersCommand { get; }

    public ICommand UseColumnHeadersCommand { get; }

    public ICommand NullColumnHeadersCommand { get; }

    public IReadOnlyList<string>? CurrentRowHeaders
    {
        get => this.currentRowHeaders;
        private set
        {
            if (ReferenceEquals(value, this.currentRowHeaders))
            {
                return;
            }

            this.currentRowHeaders = value;
            this.OnPropertyChanged();
        }
    }

    public IReadOnlyList<string>? CurrentColumnHeaders
    {
        get => this.currentColumnHeaders;
        private set
        {
            if (ReferenceEquals(value, this.currentColumnHeaders))
            {
                return;
            }

            this.currentColumnHeaders = value;
            this.OnPropertyChanged();
        }
    }

    public string? Data
    {
        get => this.data;

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

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void UpdateData()
    {
        this.Data = $"{{{string.Join(", ", this.SameLengths.Select(x => $"{{{string.Join(", ", x)}}}"))}}}";
    }
}
