using System.ComponentModel;

namespace DataGridControl.Model.Test;

public class ColumnDescriptor : INotifyPropertyChanged
{
    public required string Id { get; init; }
    public required string Header { get; set; }
    private bool _isVisible = true;
    public bool IsVisible 
    {
        get => _isVisible;
        set { if (_isVisible != value) { _isVisible = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsVisible))); } }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}