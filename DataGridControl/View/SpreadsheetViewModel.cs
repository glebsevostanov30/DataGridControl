using CommunityToolkit.Mvvm.Input;
using DataGridControl.Command.Row;
using DataGridControl.Dialog;
using DataGridControl.Model;
using DataGridControl.Service;

namespace DataGridControl.View;

public partial class SpreadsheetViewModel
{
    public SpreadsheetModel model { get; } = new();
    private readonly CommandHistory _history = CommandHistory.instance;
    private readonly SelectionColumnsDialog _selectionColumnsDialog;

    public SpreadsheetViewModel()
    {
        _selectionColumnsDialog = new SelectionColumnsDialog(model);
    }

    [RelayCommand]
    private void AddRow()
    {
        var newRow = new RowData { Col1 = "", Col2 = "", Col3 = "" };
        var command = new AddRowCommand(model, newRow, model.Rows.Count);
        _history.Execute(command);
    }

    [RelayCommand]
    private void DeleteRows()
    {
        var selected = model.SelectedRows.ToList();
        if (selected.Count == 0) return;

        var commands = selected.Select(row => new DeleteRowCommand(model, row, model.Rows.IndexOf(row)));

        _history.ExecuteGroup(commands, "Удаление нескольких строк");
        model.SelectedRows.Clear();
    }

    [RelayCommand]
    private void AddColumn()
    {
    }

    [RelayCommand]
    private void Hidden()
    {
    }

    [RelayCommand]
    private void ChooseColumn()
    {
        _selectionColumnsDialog.Show();
    }

    [RelayCommand]
    private void DeleteColumns()
    {
    }

    [RelayCommand]
    private void Undo()
    {
        _history.Undo();
    }

    [RelayCommand]
    private void Redo()
    {
        _history.Redo();
    }
}