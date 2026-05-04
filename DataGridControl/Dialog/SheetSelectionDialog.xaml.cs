using System.Data;
using System.Windows;
using System.Windows.Media;
using DataGridControl.Model.Dialog;

namespace DataGridControl.Dialog;

public partial class SheetSelectionDialog
{
    public DataTable SelectedTable { get; private set; } = new();
    public string SelectedSheetName { get; private set; } = string.Empty;
    private DataSet DataSet { get;}
    private string CurrentSheetName { get; }

    public SheetSelectionDialog(DataSet dataSet, string currentSheetName)
    {
        DataSet = dataSet;
        CurrentSheetName = currentSheetName;
        InitializeComponent();
        InitializeSheetList();
    }

    private void InitializeSheetList()
    {
        var sheetItems = new List<SheetItem>();
        var lastSheetIndex = -1;
        var currentSheetIndex = -1;

        for (var i = 0; i < DataSet.Tables.Count; i++)
        {
            var table = DataSet.Tables[i];
            var displayText = $"{table.TableName} ({table.Rows.Count} строк)";

            if (!string.IsNullOrEmpty(CurrentSheetName) && table.TableName == CurrentSheetName)
            {
                displayText += " [Текущий]";
            }

            var sheetItem = new SheetItem
            {
                Table = table,
                DisplayText = displayText,
                FontWeight = (!string.IsNullOrEmpty(CurrentSheetName) && table.TableName == CurrentSheetName)
                    ? FontWeights.Bold : FontWeights.Normal,
                TextColor = (!string.IsNullOrEmpty(CurrentSheetName) && table.TableName == CurrentSheetName)
                    ? Brushes.Blue : Brushes.Black
            };

            sheetItems.Add(sheetItem);

            if (i == DataSet.Tables.Count - 1)
            {
                lastSheetIndex = i;
            }
            if (!string.IsNullOrEmpty(CurrentSheetName) && table.TableName == CurrentSheetName)
            {
                currentSheetIndex = i;
            }
        }

        SheetListBox.ItemsSource = sheetItems;

        // Выбираем лист: сначала текущий, если есть, иначе последний
        if (sheetItems.Count <= 0) return;
        var indexToSelect = currentSheetIndex >= 0 ? currentSheetIndex : lastSheetIndex;

        if (indexToSelect < 0) return;
        SheetListBox.SelectedIndex = indexToSelect;
            
        // Автоматически выбираем лист
        if (SheetListBox.SelectedItem is not SheetItem defaultItem) return;
            
        SelectedTable = defaultItem.Table;
        SelectedSheetName = defaultItem.Table.TableName;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        // if (SheetListBox.SelectedItem is SheetItem selectedItem)
        // {
        //     var excelFinder = new ExcelFinder();
        //         
        //     var response = excelFinder.Find(new ExcelFinderRequest()
        //     {
        //         DataTable = selectedItem.Table,
        //         CancellationTokenSource = new  CancellationTokenSource()
        //     });
        //         
        //     SelectedTable = response.DataTable;
        //     SelectedSheetName = response.DataTable.TableName;
        //     DialogResult = true;
        // }
        // else
        // {
        //     MessageBoxUtil.InfoExcelSelectSheet();
        // }
    }
}