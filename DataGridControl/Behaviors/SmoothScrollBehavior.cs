using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors
{
    public class SmoothScrollBehavior : Behavior<DataGrid>
    {
        private ScrollViewer? _scrollViewer;
        private double _currentOffset;
        private double _targetOffset;
        private bool _isAnimating;
        private const double AnimationSpeed = 8.0; // Чем больше, тем быстрее

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = FindScrollViewer(AssociatedObject);
            if (_scrollViewer == null) return;
            
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
            AssociatedObject.MouseWheel += OnMouseWheelSuppress;
            _scrollViewer.ScrollChanged += OnScrollChanged;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_scrollViewer == null) return;
            
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            AssociatedObject.MouseWheel -= OnMouseWheelSuppress;
            _scrollViewer.ScrollChanged -= OnScrollChanged;
            
            // Останавливаем анимацию при откреплении
            if (!_isAnimating) return;
            CompositionTarget.Rendering -= OnRendering;
            _isAnimating = false;
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // Если пользователь скроллит вручную — синхронизируем позицию
            if (_isAnimating || _scrollViewer == null) return;
            _currentOffset = _scrollViewer.VerticalOffset;
            _targetOffset = _currentOffset;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_scrollViewer == null) return;
            e.Handled = true;

            var step = _scrollViewer.ViewportHeight * 0.9;
            var delta = (e.Delta > 0) ? -step : step;

            _targetOffset = _currentOffset + delta;
            _targetOffset = Math.Max(0, Math.Min(_targetOffset, _scrollViewer.ScrollableHeight));

            if (_isAnimating) return;
            _isAnimating = true;
            CompositionTarget.Rendering += OnRendering;
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            if (_scrollViewer == null) return;

            // Плавное приближение к цели (экспоненциальное затухание)
            _currentOffset += (_targetOffset - _currentOffset) / AnimationSpeed;

            // Применяем смещение
            _scrollViewer.ScrollToVerticalOffset(_currentOffset);

            // Если достигли цели — останавливаем анимацию
            if (!(Math.Abs(_targetOffset - _currentOffset) < 0.5)) return;
            _currentOffset = _targetOffset;
            _scrollViewer.ScrollToVerticalOffset(_currentOffset);
                
            CompositionTarget.Rendering -= OnRendering;
            _isAnimating = false;
        }

        private static void OnMouseWheelSuppress(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        private static ScrollViewer? FindScrollViewer(DependencyObject parent)
        {
            if (parent is ScrollViewer viewer) return viewer;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var result = FindScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }
    }
}