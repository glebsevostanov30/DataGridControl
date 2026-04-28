using DataGridControl.Model;
using DataGridControl.View;

namespace DataGridControl.Command.Cell;

public class ChangePropertyCommand(
    SpreadsheetViewModel vm,
    RowData target,
    string propertyName,
    object oldValue,
    object newValue)
    : IUndoRedoCommand
{
    public string Description => $"Изменение {propertyName} с '{oldValue}' на '{newValue}'";

    public void Undo()
    {
        vm.IsUndoRedoInProgress = true;
        SetProperty(oldValue);
        vm.IsUndoRedoInProgress = false;
    }

    public void Redo()
    {
        vm.IsUndoRedoInProgress = true;
        SetProperty(newValue);
        vm.IsUndoRedoInProgress = false;
    }

    private void SetProperty(object value)
    {
        var prop = typeof(RowData).GetProperty(propertyName);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(target, value);
        }
    }
}