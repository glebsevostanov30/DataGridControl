namespace DataGridControl.Model.Test;

public class RowRecord
{
    public required string Id { get; init; }
    public bool IsVisible { get; set; } = true;
    public required object[] Values { get; set; } = Array.Empty<object>();
}
