namespace DataGridControl.Behaviors;

using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

public static class HeaderEditHelper
{
    public static readonly DependencyProperty IsEditableProperty =
        DependencyProperty.RegisterAttached(
            "IsEditable", 
            typeof(bool), 
            typeof(HeaderEditHelper),
            new PropertyMetadata(false, OnIsEditableChanged));

    public static void SetIsEditable(DependencyObject element, bool value) => 
        element.SetValue(IsEditableProperty, value);
    
    public static bool GetIsEditable(DependencyObject element) => 
        (bool)element.GetValue(IsEditableProperty);

    private static void OnIsEditableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not DataGridColumnHeader header || !(bool)e.NewValue) return;
        var behaviors = Interaction.GetBehaviors(header);
        // Добавляем только если ещё нет
        if (!behaviors.OfType<InlineHeaderEditBehavior>().Any())
            behaviors.Add(new InlineHeaderEditBehavior());
    }
}