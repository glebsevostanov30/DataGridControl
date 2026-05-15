// using System.Windows;
// using System.Windows.Data;
//
// namespace DataGridControl.Behaviors;
//
// public class DataGridColumnsVisiableBehaivor
// {
//     // 2. Свойство для BoolToVisibility конвертера (опционально)
//
//     public static readonly DependencyProperty BoolToVisibilityConverterProperty =
//         DependencyProperty.RegisterAttached(
//             "BoolToVisibilityConverter",
//             typeof(IValueConverter),
//             typeof(DataGridColumnsBehavior),
//             new PropertyMetadata(null));
//
//     public static void SetBoolToVisibilityConverter(DependencyObject element, IValueConverter value) =>
//         element.SetValue(BoolToVisibilityConverterProperty, value);
//
//     public static IValueConverter GetBoolToVisibilityConverter(DependencyObject element) =>
//         (IValueConverter)element.GetValue(BoolToVisibilityConverterProperty);
// }