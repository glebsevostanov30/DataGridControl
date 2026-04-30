namespace DataGridControl.Command;

public class CompositeCommand(IEnumerable<IUndoRedoCommand> commands, string description) : IUndoRedoCommand
{
    private readonly List<IUndoRedoCommand> _commands = commands.ToList();

    public void Redo()
    {
        foreach (var c in _commands) c.Redo();
    }
    
    public void Undo()
    {
        foreach (var c in Enumerable.Reverse(_commands)) c.Undo();
    }
    
    public string Description { get; } = description;
}