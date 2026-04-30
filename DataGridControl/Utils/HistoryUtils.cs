using DataGridControl.Command;

namespace DataGridControl.Utils;

public static class HistoryUtils
{
    private static Stack<IList<IUndoRedoCommand>> UndoStack { get; set; } = new();
    private static Stack<IList<IUndoRedoCommand>> RedoStack { get; set; } = new();
}