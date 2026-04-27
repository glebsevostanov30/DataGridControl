using CommunityToolkit.Mvvm.ComponentModel;

namespace DataGridControl.Model;

public partial class RowData : ObservableObject
{
    [ObservableProperty] private string _col1 = string.Empty;
    [ObservableProperty] private string _col2 = string.Empty;
    [ObservableProperty] private string _col3 = string.Empty;

    public RowData Clone()
    {
        return new RowData
        {
            Col1 = Col1,
            Col2 = Col2,
            Col3 = Col3
        };
    }
}