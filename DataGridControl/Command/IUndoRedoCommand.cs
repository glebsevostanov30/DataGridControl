namespace DataGridControl.Command
{
    public interface IUndoRedoCommand
    {
        void Undo();
        void Redo();
        string Description { get; }
    }
}