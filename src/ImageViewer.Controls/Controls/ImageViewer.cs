using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ImageViewer.Controls.Controls
{
    [TemplatePart(Name = PART_ScrollViewer, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PART_Viewbox, Type = typeof(Viewbox))]
    [TemplatePart(Name = PART_Image, Type = typeof(Image))]
    public class ImageViewer : Control
    {
        private const string PART_ScrollViewer = "PART_ScrollViewer";
        private const string PART_Viewbox = "PART_Viewbox";
        private const string PART_Image = "PART_Image";

        #region Fields

        protected ScrollViewer _scrollViewer;
        protected Viewbox _viewbox;
        protected Image _image;

        private const double INIT_SCALE = 0.96;
        private const double INCREASE_SCALE = 1.1;
        private const double DECREASE_SCALE = 0.9;

        private Point? lastDragPoint;
        private Point? lastCenterPositionOnTarget;
        private Point? lastMousePositionOnTarget;

        #endregion Fields

        #region Dependency Properties

        public static readonly DependencyProperty ImagePathProperty
             = DependencyProperty.Register(nameof(ImagePath),
                                          typeof(string),
                                          typeof(ImageViewer),
                                          new PropertyMetadata(null, new PropertyChangedCallback(OnImagePathChanged)));

        /// <summary>Identifies the <see cref="CurrentScale"/> dependency property.</summary>
        public static readonly DependencyProperty CurrentScaleProperty
            = DependencyProperty.Register(nameof(CurrentScale),
                                          typeof(double),
                                          typeof(ImageViewer),
                                          new PropertyMetadata(1.0, new PropertyChangedCallback(OnCurrentScaleChanged)));

        #endregion Dependency Properties

        #region Property Changed

        private static void OnImagePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ImageViewer;

            if ((string)e.NewValue is null)
                return;

            control.LoadImage((string)e.NewValue);
        }

        private static void OnCurrentScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 외부에서 변경 되었을 때 실행
            var control = d as ImageViewer;
        }

        #endregion Property Changed

        #region Properties

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public double CurrentScale
        {
            get { return (double)GetValue(CurrentScaleProperty); }
            set { SetValue(CurrentScaleProperty, value); }
        }

        #endregion Properties

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _scrollViewer = GetTemplateChild(PART_ScrollViewer) as ScrollViewer;
            if (_scrollViewer != null)
            {
                _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
                _scrollViewer.PreviewMouseRightButtonDown += _scrollViewer_PreviewMouseRightButtonDown;
                _scrollViewer.MouseRightButtonUp += ScrollViewer_MouseRightButtonUp;
                _scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
                _scrollViewer.MouseMove += ScrollViewer_MouseMove;
            }
            _viewbox = GetTemplateChild(PART_Viewbox) as Viewbox;
            if (_viewbox != null)
            {
                _viewbox.Cursor = Cursors.Arrow;
                _viewbox.Loaded += Viewbox_Loaded;
            }
            _image = GetTemplateChild(PART_Image) as Image;
        }

        #region Methods

        private void LoadImage(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                _image.Source = bitmapImage;
            }
        }

        #endregion Methods

        #region ScrollViewer

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(_scrollViewer.ViewportWidth / 2, _scrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow = _scrollViewer.TranslatePoint(centerOfViewport, _viewbox);
                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(_viewbox);
                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    SetScrollBarOffset(e, targetNow, targetBefore);
                }
            }
        }

        private void SetScrollBarOffset(ScrollChangedEventArgs e, Point? targetNow, Point? targetBefore)
        {
            double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
            double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

            double multiplicatorX = e.ExtentWidth / _viewbox.Width;
            double multiplicatorY = e.ExtentHeight / _viewbox.Height;

            double newOffsetX = _scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
            double newOffsetY = _scrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

            if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                return;

            _scrollViewer.ScrollToHorizontalOffset(newOffsetX);
            _scrollViewer.ScrollToVerticalOffset(newOffsetY);
        }

        private void ScrollViewer_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _scrollViewer.Cursor = Cursors.Arrow;
            _scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (String.IsNullOrEmpty(ImagePath))
            {
                e.Handled = true;
                return;
            }

            lastMousePositionOnTarget = Mouse.GetPosition(_viewbox);
            if (e.Delta > 0)
            {
                ChangeScale(INCREASE_SCALE);
            }
            if (e.Delta < 0)
            {
                ChangeScale(DECREASE_SCALE);
            }

            e.Handled = true;
        }

        private void ChangeScale(double scale)
        {
            var tempScale = CurrentScale * scale;
            if (tempScale > 40.0)
                return;

            if (tempScale < 0.8)
                return;

            CurrentScale *= scale;
        }

        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(_scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset - dX);
                _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - dY);

                e.Handled = true;
            }
        }

        private void _scrollViewer_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(_scrollViewer);
            if (mousePos.X <= _scrollViewer.ViewportWidth &&
                mousePos.Y < _scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                _scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(_scrollViewer);
            }

            e.Handled = true;
        }

        #endregion ScrollViewer

        #region ViewBox

        private void Viewbox_Loaded(object sender, RoutedEventArgs e)
        {
            if (ImagePath is null)
                return;

            if (_viewbox is null)
                return;

            InitSclae();
        }

        private void InitSclae(double scale = 1.0)
        {
            CurrentScale = INIT_SCALE * scale;
        }

        #endregion ViewBox
    }
}