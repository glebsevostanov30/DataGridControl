using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors;

public class InlineHeaderEditBehavior : Behavior<DataGridColumnHeader>
{
    private object? _originalContent;
    private string? _originalText;

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.PreviewMouseDoubleClick += OnDoubleClick;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewMouseDoubleClick -= OnDoubleClick;
        base.OnDetaching();
    }

    private void OnDoubleClick(object sender, MouseButtonEventArgs e)
    {
        // Игнорируем, если уже редактируем или колонка пуста
        if (AssociatedObject.Column == null || _originalContent != null) return;

        _originalContent = AssociatedObject.Content;
        _originalText = AssociatedObject.Column.Header?.ToString() ?? string.Empty;

        var editBox = new TextBox
        {
            Text = _originalText,
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Padding = new Thickness(0),
            Margin = new Thickness(0),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = AssociatedObject.FontSize,
            FontWeight = AssociatedObject.FontWeight,
            Foreground = AssociatedObject.Foreground,
            FontFamily = AssociatedObject.FontFamily
        };

        editBox.LostFocus += EditBox_LostFocus;
        editBox.KeyDown += EditBox_KeyDown;

        AssociatedObject.Content = editBox;

        // Фокус и выделение текста требуют отложенного вызова (после отрисовки)
        AssociatedObject.Dispatcher.BeginInvoke(new Action(() =>
        {
            editBox.Focus();
            editBox.SelectAll();
        }), DispatcherPriority.Loaded);

        e.Handled = true;
    }

    private void EditBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) { CommitEdit(); e.Handled = true; }
        else if (e.Key == Key.Escape) { CancelEdit(); e.Handled = true; }
    }

    private void EditBox_LostFocus(object sender, RoutedEventArgs e) => CommitEdit();

    private void CommitEdit()
    {
        if (AssociatedObject.Content is not TextBox tb) return;
        
        var newName = tb.Text.Trim();
        if (!string.IsNullOrEmpty(newName))
            AssociatedObject.Column.Header = newName;
        
        ResetEdit();
    }

    private void CancelEdit()
    {
        AssociatedObject.Column.Header = _originalText;
        ResetEdit();
    }

    private void ResetEdit()
    {
        if (AssociatedObject.Content is TextBox tb)
        {
            tb.LostFocus -= EditBox_LostFocus;
            tb.KeyDown -= EditBox_KeyDown;
        }

        AssociatedObject.Content = _originalContent;
        _originalContent = null;
        _originalText = null;
    }
}