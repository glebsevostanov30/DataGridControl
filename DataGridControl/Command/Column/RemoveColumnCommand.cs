using DataGridControl.Model;
using DataGridControl.View;

namespace DataGridControl.Command.Column;

public class RemoveColumnCommand(SpreadsheetViewModel vm,
    RowData newRow,
    int index)
    : IUndoRedoCommand
{
    public string Description => $"Удаление колонки колонки в позицию {index}";

    public void Undo()
    {

    }

    public void Redo()
    {
        
    }
}