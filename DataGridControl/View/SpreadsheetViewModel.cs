using System.ComponentModel;
using System.Data;
using CommunityToolkit.Mvvm.Input;
using DataGridControl.Command;
using DataGridControl.Command.Column;
using DataGridControl.Command.Row;
using DataGridControl.Dialog;
using DataGridControl.Service;
using Microsoft.VisualBasic;

namespace DataGridControl.View;

public partial class SpreadsheetViewModel
{
    public ViewModel view { get; } = new();
    private readonly CommandHistory _history = CommandHistory.instance;
    private readonly SelectionColumnsDialog _selectionColumnsDialog;
    public event PropertyChangedEventHandler PropertyChanged;


    public SpreadsheetViewModel()
    {
    }


    [RelayCommand]
    private void AddRow()
    {
        var command = new AddRowCommand(view);
        _history.Execute(command);
    }

    public void RenameColumn(string oldName, string newName)
    {
        var command = new RenameColumnCommand(view, oldName, newName);
        _history.Execute(command);
    }

    [RelayCommand]
    private void DeleteRows()
    {
        var selected = view.SelectedRows;

        if (selected.Count <= 0) return;

        // var commands = new List<IUndoRedoCommand>();
        // foreach (DataRowView row in selected)
        // {
        //     commands.Add(
        //         new DeleteRowCommand(view, selected.GetEnumerator())
        //     );
        // }
        // var commands = selected.Select(row => new DeleteRowCommand(view, row));
        var asdf = new DeleteRowCommand(view, selected);
        _history.Execute(asdf);
    }

    [RelayCommand]
    private void DeleteRowsByCell()
    {
        // var selected = view.SelectedCells;
        //
        // if (selected.Count <= 0) return;
        // var commands = selected.Select(cell => new DeleteRowCommand(view, cell.Item as DataRowView));
        //
        // _history.ExecuteGroup(commands, "Удаление нескольких строк по ячейкам");
        // view.SelectedCells.Clear();
    }

    [RelayCommand]
    private void AddColumn()
    {
        var newColumn = new DataColumn(Guid.NewGuid().ToString());
        var command = new AddColumnCommand(view, newColumn);
        _history.Execute(command);
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
    private void DeleteColumn()
    {
    }

    [RelayCommand]
    private void DeleteColumnByCell()
    {
        var selected = view.SelectedCells;

        if (selected.Count <= 0) return;
        var commands = selected.Select(cell => new DeleteColumnCommand(view, cell.Column));

        _history.ExecuteGroup(commands, "Удаление нескольких строк по ячейкам");
        view.SelectedCells.Clear();
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