using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Xaml.Behaviors;

namespace DataGridControl.Behaviors
{
    public class SmoothScrollBehavior : Behavior<DataGrid>
    {
        // === Регистрация зависимых свойств ===

        public static readonly DependencyProperty VerticalAnimationSpeedProperty =
            DependencyProperty.Register(nameof(VerticalAnimationSpeed), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(8.0));
        
        public static readonly DependencyProperty HorizontalAnimationSpeedProperty =
            DependencyProperty.Register(nameof(HorizontalAnimationSpeed), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(8.0));

        public static readonly DependencyProperty VerticalBaseStepCountProperty =
            DependencyProperty.Register(nameof(VerticalBaseStepCount), typeof(int), typeof(SmoothScrollBehavior),
                new PropertyMetadata(3));
        
        public static readonly DependencyProperty HorizontalBaseStepCountProperty =
            DependencyProperty.Register(nameof(HorizontalBaseStepCount), typeof(int), typeof(SmoothScrollBehavior),
                new PropertyMetadata(3));

        public static readonly DependencyProperty VerticalMaxStepCountProperty =
            DependencyProperty.Register(nameof(VerticalMaxStepCount), typeof(int), typeof(SmoothScrollBehavior),
                new PropertyMetadata(50));
        
        public static readonly DependencyProperty HorizontalMaxStepCountProperty =
            DependencyProperty.Register(nameof(HorizontalMaxStepCount), typeof(int), typeof(SmoothScrollBehavior),
                new PropertyMetadata(50));

        public static readonly DependencyProperty VerticalUnitScrollRatioProperty =
            DependencyProperty.Register(nameof(VerticalUnitScrollRatio), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(0.05));
        
        public static readonly DependencyProperty HorizontalUnitScrollRatioProperty =
            DependencyProperty.Register(nameof(HorizontalUnitScrollRatio), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(0.05));

        public static readonly DependencyProperty VerticalAccelerationThresholdProperty =
            DependencyProperty.Register(nameof(VerticalAccelerationThreshold), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(200.0));
        
        public static readonly DependencyProperty HorizontalAccelerationThresholdProperty =
            DependencyProperty.Register(nameof(HorizontalAccelerationThreshold), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(200.0));

        public static readonly DependencyProperty VerticalAccelerationIncrementProperty =
            DependencyProperty.Register(nameof(VerticalAccelerationIncrement), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(2.0));
        
        public static readonly DependencyProperty HorizontalAccelerationIncrementProperty =
            DependencyProperty.Register(nameof(HorizontalAccelerationIncrement), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(2.0));

        public static readonly DependencyProperty VerticalAccelerationDecayProperty =
            DependencyProperty.Register(nameof(VerticalAccelerationDecay), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(0.96));

        public static readonly DependencyProperty HorizontalAccelerationDecayProperty =
            DependencyProperty.Register(nameof(HorizontalAccelerationDecay), typeof(double), typeof(SmoothScrollBehavior),
                new PropertyMetadata(0.96));

        // === CLR-обёртки для доступа к свойствам ===
        public double VerticalAnimationSpeed
        {
            get => (double)GetValue(VerticalAnimationSpeedProperty);
            set => SetValue(VerticalAnimationSpeedProperty, value);
        }
        
        public double HorizontalAnimationSpeed
        {
            get => (double)GetValue(HorizontalAnimationSpeedProperty);
            set => SetValue(HorizontalAnimationSpeedProperty, value);
        }

        public int VerticalBaseStepCount
        {
            get => (int)GetValue(VerticalBaseStepCountProperty);
            set => SetValue(VerticalBaseStepCountProperty, value);
        }
        
        public int HorizontalBaseStepCount
        {
            get => (int)GetValue(HorizontalBaseStepCountProperty);
            set => SetValue(HorizontalBaseStepCountProperty, value);
        }

        public int VerticalMaxStepCount
        {
            get => (int)GetValue(VerticalMaxStepCountProperty);
            set => SetValue(VerticalMaxStepCountProperty, value);
        }
        
        public int HorizontalMaxStepCount
        {
            get => (int)GetValue(HorizontalMaxStepCountProperty);
            set => SetValue(HorizontalMaxStepCountProperty, value);
        }

        public double VerticalUnitScrollRatio
        {
            get => (double)GetValue(VerticalUnitScrollRatioProperty);
            set => SetValue(VerticalUnitScrollRatioProperty, value);
        }
        
        public double HorizontalUnitScrollRatio
        {
            get => (double)GetValue(HorizontalUnitScrollRatioProperty);
            set => SetValue(HorizontalUnitScrollRatioProperty, value);
        }

        public double VerticalAccelerationThreshold
        {
            get => (double)GetValue(VerticalAccelerationThresholdProperty);
            set => SetValue(VerticalAccelerationThresholdProperty, value);
        }
        
        public double HorizontalAccelerationThreshold
        {
            get => (double)GetValue(HorizontalAccelerationThresholdProperty);
            set => SetValue(HorizontalAccelerationThresholdProperty, value);
        }

        public double VerticalAccelerationIncrement
        {
            get => (double)GetValue(VerticalAccelerationIncrementProperty);
            set => SetValue(VerticalAccelerationIncrementProperty, value);
        }

        public double HorizontalAccelerationIncrement
        {
            get => (double)GetValue(HorizontalAccelerationIncrementProperty);
            set => SetValue(HorizontalAccelerationIncrementProperty, value);
        }

        public double VerticalAccelerationDecay
        {
            get => (double)GetValue(VerticalAccelerationDecayProperty);
            set => SetValue(VerticalAccelerationDecayProperty, value);
        }
        
        public double HorizontalAccelerationDecay
        {
            get => (double)GetValue(HorizontalAccelerationDecayProperty);
            set => SetValue(HorizontalAccelerationDecayProperty, value);
        }

        // === Внутреннее состояние ===
        private ScrollViewer _scrollViewer;
        private double _currentVerticalOffset;
        private double _targetVerticalOffset;
        private bool _isAnimating;

        // === Внутреннее состояние ===
        private double _currentHorizontalOffset;
        private double _targetHorizontalOffset;

        private DateTime _lastWheelTime;
        private double _stepVerticalMultiplier;
        private double _stepHorizontalMultiplier;
        private readonly DispatcherTimer _verticalDecayTimer = new() { Interval = TimeSpan.FromMilliseconds(450) };

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = FindScrollViewer(AssociatedObject);
            _scrollViewer = scrollViewer ?? throw new Exception("Could not find ScrollViewer for parent");

            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
            AssociatedObject.MouseWheel += OnMouseWheelSuppress;
            _scrollViewer.ScrollChanged += OnScrollChanged;

            _stepVerticalMultiplier = VerticalBaseStepCount;
            _stepHorizontalMultiplier = HorizontalBaseStepCount;
            _lastWheelTime = DateTime.MinValue;

            _verticalDecayTimer.Tick += OnVerticalDecayTimerTick;
            _verticalDecayTimer.Start();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            AssociatedObject.MouseWheel -= OnMouseWheelSuppress;
            _scrollViewer.ScrollChanged -= OnScrollChanged;

            _verticalDecayTimer.Stop();

            if (!_isAnimating) return;
            CompositionTarget.Rendering -= OnHorizontalRendering;
            CompositionTarget.Rendering -= OnVerticalRendering;
            _isAnimating = false;
        }

        private void OnVerticalDecayTimerTick(object? sender, EventArgs e)
        {
            if (!(_stepVerticalMultiplier > VerticalBaseStepCount)) return;

            _stepVerticalMultiplier *= VerticalAccelerationDecay;
            if (_stepVerticalMultiplier < VerticalBaseStepCount + 0.1)
                _stepVerticalMultiplier = VerticalBaseStepCount;
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_isAnimating) return;
            _currentVerticalOffset = _scrollViewer.VerticalOffset;
            _targetVerticalOffset = _currentVerticalOffset;
            
            _currentHorizontalOffset = _scrollViewer.HorizontalOffset;
            _targetHorizontalOffset = _currentHorizontalOffset;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            
            // Расчёт ускорения
            var now = DateTime.Now;
            var deltaMs = (now - _lastWheelTime).TotalMilliseconds;
            _lastWheelTime = now;
            
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {   
                if (deltaMs < HorizontalAccelerationThreshold)
                {
                    _stepHorizontalMultiplier += HorizontalAccelerationIncrement;
                    _stepHorizontalMultiplier = Math.Min(_stepHorizontalMultiplier, HorizontalMaxStepCount);
                }
                // === Горизонтальный скролл (Shift) ===
                var unitHorizontalStep = _scrollViewer.ViewportWidth * HorizontalUnitScrollRatio;
                var horizontalStep = unitHorizontalStep * _stepHorizontalMultiplier;
                var horizontalDelta = (e.Delta > 0) ? -horizontalStep : horizontalStep;

                _targetHorizontalOffset = _currentHorizontalOffset + horizontalDelta;
                _targetHorizontalOffset = Math.Max(0, Math.Min(_targetHorizontalOffset, _scrollViewer.ScrollableWidth));
        
                if (_isAnimating) return;
                _isAnimating = true;
                CompositionTarget.Rendering += OnHorizontalRendering;
                Debug.WriteLine($"🎯 Horizontal: target={_targetVerticalOffset:F1}, current={_currentVerticalOffset:F1}, stepMult={_stepVerticalMultiplier:F2}");
                return;
            }
            
            if (deltaMs < VerticalAccelerationThreshold)
            {
                _stepVerticalMultiplier += VerticalAccelerationIncrement;
                _stepVerticalMultiplier = Math.Min(_stepVerticalMultiplier, VerticalMaxStepCount);
            }
    
            // === Вертикальный скролл (без Shift) ===
            var unitVerticalStep = _scrollViewer.ViewportHeight * VerticalUnitScrollRatio;
            var verticalStep = unitVerticalStep * _stepVerticalMultiplier;
            var verticalDelta = (e.Delta > 0) ? -verticalStep : verticalStep;

            _targetVerticalOffset = _currentVerticalOffset + verticalDelta;
            _targetVerticalOffset = Math.Max(0, Math.Min(_targetVerticalOffset, _scrollViewer.ScrollableHeight));
    
            if (_isAnimating) return;
            _isAnimating = true;
            CompositionTarget.Rendering += OnVerticalRendering;
            Debug.WriteLine($"🎯 Vertical: target={_targetVerticalOffset:F1}, current={_currentVerticalOffset:F1}, stepMult={_stepVerticalMultiplier:F2}");
        }

        private void OnHorizontalRendering(object? sender, EventArgs e)
        {
            _currentHorizontalOffset += (_targetHorizontalOffset - _currentHorizontalOffset) / HorizontalAnimationSpeed;
            _scrollViewer.ScrollToHorizontalOffset(_currentHorizontalOffset);

            if (!(Math.Abs(_targetHorizontalOffset - _currentHorizontalOffset) < 0.5)) return;
            _currentHorizontalOffset = _targetHorizontalOffset;
            _scrollViewer.ScrollToHorizontalOffset(_currentHorizontalOffset);

            CompositionTarget.Rendering -= OnHorizontalRendering;
            _isAnimating = false;
        }
        
        private void OnVerticalRendering(object? sender, EventArgs e)
        {
            _currentVerticalOffset += (_targetVerticalOffset - _currentVerticalOffset) / VerticalAnimationSpeed;
            _scrollViewer.ScrollToVerticalOffset(_currentVerticalOffset);

            if (!(Math.Abs(_targetVerticalOffset - _currentVerticalOffset) < 0.5)) return;
            _currentVerticalOffset = _targetVerticalOffset;
            _scrollViewer.ScrollToVerticalOffset(_currentVerticalOffset);

            CompositionTarget.Rendering -= OnVerticalRendering;
            _isAnimating = false;
        }

        private static void OnMouseWheelSuppress(object sender, MouseWheelEventArgs e) => e.Handled = true;

        private static ScrollViewer? FindScrollViewer(DependencyObject parent)
        {
            if (parent is ScrollViewer viewer) return viewer;
    
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var result = FindScrollViewer(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}