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
        
        public static readonly DependencyProperty AnimationSpeedProperty =
            DependencyProperty.Register(nameof(AnimationSpeed), typeof(double), typeof(SmoothScrollBehavior), 
                new PropertyMetadata(8.0));
        
        public static readonly DependencyProperty BaseStepCountProperty =
            DependencyProperty.Register(nameof(BaseStepCount), typeof(int), typeof(SmoothScrollBehavior), 
                new PropertyMetadata(3));
        
        public static readonly DependencyProperty MaxStepCountProperty =
            DependencyProperty.Register(nameof(MaxStepCount), typeof(int), typeof(SmoothScrollBehavior), 
                new PropertyMetadata(50));
        
        public static readonly DependencyProperty UnitScrollRatioProperty =
            DependencyProperty.Register(nameof(UnitScrollRatio), typeof(double), typeof(SmoothScrollBehavior), 
                new PropertyMetadata(0.05));
        
        public static readonly DependencyProperty AccelerationThresholdProperty =
            DependencyProperty.Register(nameof(AccelerationThreshold), typeof(double), typeof(SmoothScrollBehavior), 
                new PropertyMetadata(200.0));
        
        public static readonly DependencyProperty AccelerationIncrementProperty =
            DependencyProperty.Register(nameof(AccelerationIncrement), typeof(double), typeof(SmoothScrollBehavior), 
                new PropertyMetadata(2.0));
        
        public static readonly DependencyProperty AccelerationDecayProperty =
            DependencyProperty.Register(nameof(AccelerationDecay), typeof(double), typeof(SmoothScrollBehavior), 
                new PropertyMetadata(0.96));

        // === CLR-обёртки для доступа к свойствам ===
        
        public double AnimationSpeed
        {
            get => (double)GetValue(AnimationSpeedProperty);
            set => SetValue(AnimationSpeedProperty, value);
        }
        
        public int BaseStepCount
        {
            get => (int)GetValue(BaseStepCountProperty);
            set => SetValue(BaseStepCountProperty, value);
        }
        
        public int MaxStepCount
        {
            get => (int)GetValue(MaxStepCountProperty);
            set => SetValue(MaxStepCountProperty, value);
        }
        
        public double UnitScrollRatio
        {
            get => (double)GetValue(UnitScrollRatioProperty);
            set => SetValue(UnitScrollRatioProperty, value);
        }
        
        public double AccelerationThreshold
        {
            get => (double)GetValue(AccelerationThresholdProperty);
            set => SetValue(AccelerationThresholdProperty, value);
        }
        
        public double AccelerationIncrement
        {
            get => (double)GetValue(AccelerationIncrementProperty);
            set => SetValue(AccelerationIncrementProperty, value);
        }
        
        public double AccelerationDecay
        {
            get => (double)GetValue(AccelerationDecayProperty);
            set => SetValue(AccelerationDecayProperty, value);
        }

        // === Внутреннее состояние ===
        private ScrollViewer? _scrollViewer;
        private double _currentOffset;
        private double _targetOffset;
        private bool _isAnimating;
        
        private DateTime _lastWheelTime;
        private double _stepMultiplier;
        private DispatcherTimer? _decayTimer;

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
            
            _stepMultiplier = BaseStepCount;
            _lastWheelTime = DateTime.MinValue;
            
            _decayTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(350) };
            _decayTimer.Tick += OnDecayTimerTick;
            _decayTimer.Start();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_scrollViewer == null) return;
            
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            AssociatedObject.MouseWheel -= OnMouseWheelSuppress;
            _scrollViewer.ScrollChanged -= OnScrollChanged;
            
            _decayTimer?.Stop();
            _decayTimer = null;
            
            if (_isAnimating)
            {
                CompositionTarget.Rendering -= OnRendering;
                _isAnimating = false;
            }
        }

        private void OnDecayTimerTick(object? sender, EventArgs e)
        {
            if (_stepMultiplier > BaseStepCount)
            {
                _stepMultiplier *= AccelerationDecay;
                if (_stepMultiplier < BaseStepCount + 0.1)
                    _stepMultiplier = BaseStepCount;
            }
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_isAnimating || _scrollViewer == null) return;
            _currentOffset = _scrollViewer.VerticalOffset;
            _targetOffset = _currentOffset;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_scrollViewer == null) return;
            e.Handled = true;

            // Расчёт ускорения
            var now = DateTime.Now;
            var deltaMs = (now - _lastWheelTime).TotalMilliseconds;
            _lastWheelTime = now;

            if (deltaMs < AccelerationThreshold)
            {
                _stepMultiplier += AccelerationIncrement;
                _stepMultiplier = Math.Min(_stepMultiplier, MaxStepCount);
            }

            // Расчёт шага
            var unitStep = _scrollViewer.ViewportHeight * UnitScrollRatio;
            var step = unitStep * _stepMultiplier;
            var delta = (e.Delta > 0) ? -step : step;

            _targetOffset = _currentOffset + delta;
            _targetOffset = Math.Max(0, Math.Min(_targetOffset, _scrollViewer.ScrollableHeight));

            if (!_isAnimating)
            {
                _isAnimating = true;
                CompositionTarget.Rendering += OnRendering;
            }
            
            Debug.WriteLine(
                $"[Scroll] MultiplierF1: {_stepMultiplier:F1}, MultiplierF2: {_stepMultiplier:F2}, Step: {step:F0}px");
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            if (_scrollViewer == null) return;

            _currentOffset += (_targetOffset - _currentOffset) / AnimationSpeed;
            _scrollViewer.ScrollToVerticalOffset(_currentOffset);

            if (Math.Abs(_targetOffset - _currentOffset) < 0.5)
            {
                _currentOffset = _targetOffset;
                _scrollViewer.ScrollToVerticalOffset(_currentOffset);
                
                CompositionTarget.Rendering -= OnRendering;
                _isAnimating = false;
            }
        }

        private static void OnMouseWheelSuppress(object sender, MouseWheelEventArgs e) => e.Handled = true;

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