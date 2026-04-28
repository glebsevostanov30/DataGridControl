using DataGridControl.Model;
using DataGridControl.View;

namespace DataGridControl.Command.Column;

public class AddColumnCommand(SpreadsheetViewModel vm,
    RowData newRow,
    int index)
    : IUndoRedoCommand
{
    public string Description => $"Добавление колонки в позицию {index}";

    public void Undo()
    {

    }

    public void Redo()
    {
        
    }
}