using DataGridControl.Command;

namespace DataGridControl.Service;

public class CommandHistory
{
    private readonly Stack<IUndoRedoCommand> _undoStack = new();
    private readonly Stack<IUndoRedoCommand> _redoStack = new();
    
    // Статический экземпляр (если нужен глобальный доступ)
    public static CommandHistory instance { get; } = new();
    
    private CommandHistory() { }

    public void Execute(IUndoRedoCommand command)
    {
        command.Redo();
        _undoStack.Push(command);
        _redoStack.Clear();
        // Оповещение UI (можно через событие)
        OnHistoryChanged();
    }

    public void Undo()
    {
        if (_undoStack.Count == 0) return;
        var command = _undoStack.Pop();
        command.Undo();
        _redoStack.Push(command);
        OnHistoryChanged();
    }

    public void Redo()
    {
        if (_redoStack.Count == 0) return;
        var command = _redoStack.Pop();
        command.Redo();
        _undoStack.Push(command);
        OnHistoryChanged();
    }
    
    public void ExecuteGroup(IEnumerable<IUndoRedoCommand> commands, string description)
    {
        var group = new CompositeCommand(commands, description);
        Execute(group);
    }

    public event EventHandler? HistoryChanged;
    protected virtual void OnHistoryChanged() => HistoryChanged?.Invoke(this, EventArgs.Empty);
}