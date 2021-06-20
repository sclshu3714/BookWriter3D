using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace BookWriter3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Represents whether the book is open or not.
        /// </summary>
        bool IsBookOpen { get; set; } = false;
        public int BookmarkPage { get; set; } = 1;//书签显示页
        public int CurrentPage { get; set; } = 1;//左边的页数，起始为1页
        public int PageCount { get; set; } = 15;     //总页数
        public bool IsTurnpage { get; set; } = true;//显示翻页效果
        public List<BitmapImage> BookPages = new List<BitmapImage>();
        /// <summary>
        /// Public constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for the Loaded event of the MainWindow
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CloseBook(0);  // Book starts closed

            for (int i = 1; i <= 15; i++)
            {
                BookPages.Add(new BitmapImage(new Uri($@"pack://application:,,,/BookWriter3D;component/Resources/Images/page{i}.jpg", UriKind.RelativeOrAbsolute)));
            }
            //设置起始默认值
            this.imgLeftPage.ImageSource = BookPages[BookmarkPage - 1];
            if (BookmarkPage + 1 < BookPages.Count)
            {
                this.imgRightPage.ImageSource = BookPages[BookmarkPage];
                this.imgTurnUpPage.ImageSource = BookPages[BookmarkPage];
            }
            if (BookmarkPage + 1 < BookPages.Count)
            {
                this.imgTurnDownPage.ImageSource = BookPages[BookmarkPage + 1];
            }
            // Make book fade in
            DoubleAnimation da = new DoubleAnimation(0, 5, new Duration(TimeSpan.FromSeconds(2)));
            da.DecelerationRatio = 1;
            _Main3D.BeginAnimation(UIElement.OpacityProperty, da);
        }
        
        /// <summary>
        /// Event handler for the MouseDown event of the cover, back cover, spine and edges
        /// </summary>
        private void Cover_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsBookOpen)
                CloseBook(1.5);
            else
                OpenBook(1.5);
        }

        /// <summary>
        /// Event handler for the MouseDoubleClick event of the TextBoxes
        /// </summary>
        private void Page_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Read page content out loud
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SpeakAsync(((TextBox)sender).Text);
        }

        /// <summary>
        /// Event handler for the PreviewMouseRightButtonDown event of the InkCanvas (right page)
        /// </summary>
        private void InkCanvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Switch InkCanvas editing mode
            InkCanvas ic = sender as InkCanvas;
            ic.EditingMode = (ic.EditingMode == InkCanvasEditingMode.InkAndGesture) ? InkCanvasEditingMode.EraseByPoint : InkCanvasEditingMode.InkAndGesture;
        }

        /// <summary>
        /// Opens the 3D book.
        /// </summary>
        /// <param name="durationSeconds">Time in seconds that the animation will take.</param>
        void OpenBook(double durationSeconds)
        {
            // Transform3D_LeftRotation
            RotateTransform3D rot = (RotateTransform3D)TryFindResource("Transform3D_LeftRotation");
            DoubleAnimation da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            da.DecelerationRatio = 1;
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
            //跳转到书签页 Transform3D_LeftPageRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_LeftPageRotation");
            da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            da.DecelerationRatio = 1;
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            // Transform3D_RightRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_RightRotation");
            da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            // Transform3D_SpineRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_SpineRotation");
            da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.8333 * durationSeconds)));
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            // Transform3D_SpineCoverTranslation
            TranslateTransform3D trans = (TranslateTransform3D)TryFindResource("Transform3D_SpineCoverTranslation");
            da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.8333 * durationSeconds)));
            trans.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);

            // _Main3D.Camera
            Point3DAnimation pa = new Point3DAnimation(new Point3D(0, -2.5, 6.5), new Duration(TimeSpan.FromSeconds(durationSeconds)));
            pa.AccelerationRatio = 0.5;
            pa.DecelerationRatio = 0.5;
            ((PerspectiveCamera)_Main3D.Camera).BeginAnimation(PerspectiveCamera.PositionProperty, pa);

            // Now the book is open.
            IsBookOpen = true;
        }

        /// <summary>
        /// Closes the 3D book.
        /// </summary>
        /// <param name="durationSeconds">Time in seconds that the animation will take.</param>
        void CloseBook(double durationSeconds)
        {
            // Transform3D_LeftRotation
            RotateTransform3D rot = (RotateTransform3D)TryFindResource("Transform3D_LeftRotation");
            DoubleAnimation da = new DoubleAnimation(180, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            da.DecelerationRatio = 1;
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            //跳转到书签页 Transform3D_LeftPageRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_LeftPageRotation");
            da = new DoubleAnimation(180, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            da.DecelerationRatio = 1;
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            // Transform3D_RightRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_RightRotation");
            da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            // Transform3D_RightPageRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_RightPageRotation");
            da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            da.DecelerationRatio = 1;
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            // Transform3D_RightPageRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_TurnUpPageRotation");
            da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            da.DecelerationRatio = 1;
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
            // Transform3D_RightPageRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_TurnDownPageRotation");
            da = new DoubleAnimation(180, new Duration(TimeSpan.FromSeconds(durationSeconds)));
            da.DecelerationRatio = 1;
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            // Transform3D_SpineRotation
            rot = (RotateTransform3D)TryFindResource("Transform3D_SpineRotation");
            da = new DoubleAnimation(90, new Duration(TimeSpan.FromSeconds(0.8333 * durationSeconds)));
            rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

            // Transform3D_SpineCoverTranslation
            TranslateTransform3D trans = (TranslateTransform3D)TryFindResource("Transform3D_SpineCoverTranslation");
            da = new DoubleAnimation(-0.125, new Duration(TimeSpan.FromSeconds(0.8333 * durationSeconds)));
            trans.BeginAnimation(TranslateTransform3D.OffsetXProperty, da);

            // _Main3D.Camera
            Point3DAnimation pa = new Point3DAnimation(new Point3D(0.72, -2.5, 6.5), new Duration(TimeSpan.FromSeconds(durationSeconds)));
            pa.AccelerationRatio = 0.5;
            pa.DecelerationRatio = 0.5;
            ((PerspectiveCamera)_Main3D.Camera).BeginAnimation(PerspectiveCamera.PositionProperty, pa);

            // Now the book is closed.
            IsBookOpen = false;
        }

        /// <summary>
        /// 关闭书籍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseBook(object sender, MouseButtonEventArgs e)
        {
            CloseBook(1.5);
        }

        public void SetTurnPage(bool IsDockRight = true) {
            if (IsDockRight) {
                RotateTransform3D rot = (RotateTransform3D)TryFindResource("Transform3D_TurnUpPageRotation");
                DoubleAnimation da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0)));
                rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

                rot = (RotateTransform3D)TryFindResource("Transform3D_TurnDownPageRotation");
                da = new DoubleAnimation(180, new Duration(TimeSpan.FromSeconds(0)));
                rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
            }
            else {
                RotateTransform3D rot = (RotateTransform3D)TryFindResource("Transform3D_TurnUpPageRotation");
                DoubleAnimation da = new DoubleAnimation(-180, new Duration(TimeSpan.FromSeconds(0)));
                rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

                rot = (RotateTransform3D)TryFindResource("Transform3D_TurnDownPageRotation");
                da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0)));
                rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
            }
            this._LayoutRoot.UpdateLayout();
        }

        /// <summary>
        /// 点击左边的页（上一页或者关闭书籍）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTurnLeftPage(object sender, MouseButtonEventArgs e)
        {
            if (!IsBookOpen)
                return;
            if (CurrentPage == 1)
            {
                CloseBook(1.5);
            }
            else
            {
                CurrentPage -= 2;
                double durationSeconds = 1.5;
                if (IsTurnpage)
                {
                    SetTurnPage(false);
                    this.imgTurnUpPage.ImageSource = BookPages[CurrentPage];
                    this.imgTurnDownPage.ImageSource = this.imgLeftPage.ImageSource;
                    //显示翻页控件
                    this.txtTurnUpPage.Visibility = Visibility.Visible;
                    this.txtTurnDownPage.Visibility = Visibility.Visible;
                    //左边设置为最新要显示的页
                    this.imgLeftPage.ImageSource = BookPages[CurrentPage - 1];

                    RotateTransform3D rot = (RotateTransform3D)TryFindResource("Transform3D_TurnUpPageRotation");
                    DoubleAnimation da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(durationSeconds)));
                    da.DecelerationRatio = 1;
                    rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
                    rot = (RotateTransform3D)TryFindResource("Transform3D_TurnDownPageRotation");
                    da = new DoubleAnimation(180, new Duration(TimeSpan.FromSeconds(durationSeconds)));
                    da.DecelerationRatio = 1;
                    da.Completed += (csender, se) =>
                    {
                        this.imgRightPage.ImageSource = this.imgTurnUpPage.ImageSource;
                        this.imgTurnDownPage.ImageSource = BookPages[CurrentPage + 1];
                        this.txtTurnUpPage.Visibility = Visibility.Collapsed;
                        this.txtTurnDownPage.Visibility = Visibility.Collapsed;
                    };
                    rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
                }
            }
        }

        /// <summary>
        /// 点击右边的页（下一页或者关闭书籍）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTurnRightPage(object sender, MouseButtonEventArgs e)
        {
            if (!IsBookOpen)
                return;
            if (CurrentPage == PageCount || CurrentPage + 1 == PageCount)
            {
                CloseBook(1.5);
            }
            else
            {
                CurrentPage += 2;
                double durationSeconds = 1.5;
                //this.imgLeftPage.ImageSource = BookPages[CurrentPage];
                this.imgRightPage.ImageSource = (BookPages.Count <= CurrentPage) ? new BitmapImage(new Uri($@"pack://application:,,,/BookWriter3D;component/Resources/Images/frontpage.jpg", UriKind.RelativeOrAbsolute)) : BookPages[CurrentPage];
                if (IsTurnpage)
                {

                    this.txtTurnUpPage.Visibility = Visibility.Visible;
                    this.txtTurnDownPage.Visibility = Visibility.Visible;
                    RotateTransform3D rot = (RotateTransform3D)TryFindResource("Transform3D_TurnUpPageRotation");
                    DoubleAnimation da = new DoubleAnimation(-180, new Duration(TimeSpan.FromSeconds(durationSeconds)));
                    da.DecelerationRatio = 1;
                    rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

                    rot = (RotateTransform3D)TryFindResource("Transform3D_TurnDownPageRotation");
                    da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(durationSeconds)));
                    da.DecelerationRatio = 1;
                    da.Completed += (csender,se)=> {
                        this.imgLeftPage.ImageSource = this.imgTurnDownPage.ImageSource;

                        rot = (RotateTransform3D)TryFindResource("Transform3D_TurnUpPageRotation");
                        da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0)));
                        rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

                        rot = (RotateTransform3D)TryFindResource("Transform3D_TurnDownPageRotation");
                        da = new DoubleAnimation(180, new Duration(TimeSpan.FromSeconds(0)));
                        rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);

                        this.imgTurnUpPage.ImageSource = (BookPages.Count <= CurrentPage) ? null : BookPages[CurrentPage];
                        if (BookPages.Count > CurrentPage + 1)
                            this.imgTurnDownPage.ImageSource = BookPages[CurrentPage + 1];

                        this.txtTurnUpPage.Visibility = Visibility.Collapsed;
                        this.txtTurnDownPage.Visibility = Visibility.Collapsed;
                    };
                    rot.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
                }
            }
        }
    }
}
