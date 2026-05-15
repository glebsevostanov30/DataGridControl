using System.ComponentModel;
using System.Data;
using CommunityToolkit.Mvvm.Input;
using DataGridControl.Command.Column;
using DataGridControl.Command.Row;
using DataGridControl.Dialog;
using DataGridControl.Service;

namespace DataGridControl.View;

public partial class SpreadsheetViewModel
{
    public VeiwModel veiw { get; } = new();
    private readonly CommandHistory _history = CommandHistory.instance;
    private readonly SelectionColumnsDialog _selectionColumnsDialog;
    public SpreadsheetViewModel()
    {
        // for (int i = 0; i < 100; i++)
        // {
        //     model.Columns.Add(new ColumnDescriptor
        //     {
        //         Id = "1",
        //         Header = "Первая колонка"
        //     });
        // }
        //
        //
        // for (int i = 0; i < 10_000; i++)
        // {
        //     model.Rows.Add(new RowRecord
        //     {
        //         Id = Guid.NewGuid().ToString(),
        //         Values =
        //         [
        //         ]
        //     });
        // }
        //
        // model.Columns.Add(new ColumnDescriptor
        // {
        //     Id = "1",
        //     Header = "Первая колонка"
        // });
        //
        // model.Columns.Add(new ColumnDescriptor
        // {
        //     Id = "2",
        //     Header = "Вторая колонка"
        // });
        //
        // model.Columns.Add(new ColumnDescriptor
        // {
        //     Id = "3",
        //     Header = "Третья колонка"
        // });
        //
        // model.Rows.Add(new RowRecord
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     Values =
        //     [
        //         "a1", "b1", "c1"
        //     ]
        // });
        //
        // model.Rows.Add(new RowRecord
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     Values =
        //     [
        //         "a2", "b2", "c2"
        //     ]
        // });
        //
        // model.Rows.Add(new RowRecord
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     Values =
        //     [
        //         "a3", "b3", "c3"
        //     ]
        // });
        //
        //
        // RowsView = CollectionViewSource.GetDefaultView(model.Rows);
        // RowsView.Filter = obj => ((RowRecord)obj).IsVisible;

        // Синхронизация видимости колонок при изменении в Data
        // model.PropertyChanged += (s, e) =>
        // {
        //     switch (e.PropertyName)
        //     {
        //         case nameof(DynamicTable.Columns):
        //             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Columns)));
        //             break;
        //     }
        // };
    }

    public event PropertyChangedEventHandler PropertyChanged;


    [RelayCommand]
    private void AddRow()
    {
        // var newRow = new RowRecord
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     Values = []
        // };
        // var command = new AddRowCommand(model, newRow);
        // _history.Execute(command);

        var newRow = veiw.TableData.NewRow();
        var command = new AddRowCommand(veiw, newRow);
        _history.Execute(command);
    }

    [RelayCommand]
    private void DeleteRows()
    {
        // var selected = model.SelectedRows;
        //
        // if (selected.Count <= 0) return;
        // var commands = selected.Select(row => new DeleteRowCommand(model, row));
        //
        // _history.ExecuteGroup(commands, "Удаление нескольких строк");
        // model.SelectedRows.Clear();
    }

    [RelayCommand]
    private void AddColumn()
    {
        // var newColumn = new ColumnDescriptor
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     Header = "Новая колонка"
        // };
        //
        // var command = new AddColumnCommand(model, newColumn);
        // _history.Execute(command);
        var newColumn = new DataColumn(Guid.NewGuid().ToString());
        var command = new AddColumnCommand(veiw, newColumn);
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