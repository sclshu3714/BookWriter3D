using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EBookApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public int BookmarkPage { get; set; } = 1;//书签显示页
        public int CurrentPage { get; set; } = 1;//左边的页数，起始为1页
        public int PageCount { get; set; } = 15;     //总页数
        public List<Image> BookPages = new List<Image>();
        public MainWindow()
        {
            InitializeComponent();
            this.myBook.Items.Clear();
            for (int i = 1; i <= 15; i++)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri($@"pack://application:,,,/EBookApp;component/Resources/Images/page{i}.jpg", UriKind.RelativeOrAbsolute));
                image.Stretch = Stretch.Fill;
                BookPages.Add(image);
            }
            this.myBook.ItemsSource = BookPages;
            //int start = BookmarkPage - 2 < 0 ? 0 : BookmarkPage - 2;
            //for (int i = start; i < BookmarkPage; i++)
            //{
            //    this.myBook.Items.Add(BookPages[i]);
            //}
            //int end = BookmarkPage + 2 < 0 ? 0 : BookmarkPage + 2;
            //for (int i = BookmarkPage; i < end; i++)
            //{
            //    this.myBook.Items.Add(BookPages[i]);
            //}
            //CurrentPage = BookmarkPage;
            //this.myBook.CurrentSheetIndex = BookmarkPage;
        }

        private void AutoNextClick(object sender, RoutedEventArgs e)
        {
            CurrentPage += 1;
            //if (BookPages.Count > CurrentPage + 1)
            //{
            //    this.myBook.Dispatcher.Invoke(new Action(() => {
            //        this.myBook.Items.Add(BookPages[CurrentPage + 1]);
            //        this.myBook.UpdateLayout();
            //    }));
            //}
            myBook.AnimateToNextPage(false, 700);
            myBook.Focus();
            //if (BookPages.Count > CurrentPage + 2 && this.myBook.Items.Count > 5)
            //{
            //    this.myBook.Dispatcher.Invoke(new Action(() => {
            //        this.myBook.Items.Remove(BookPages[CurrentPage - 4]);
            //        this.myBook.UpdateLayout();
            //    }));
            //}
        }

        private void AutoPreviousClick(object sender, RoutedEventArgs e)
        {
            myBook.AnimateToPreviousPage(false, 700);
            myBook.Focus();
            //CurrentPage -= 1;
            //if (CurrentPage - 1 > 0)
            //{
            //    this.myBook.Dispatcher.Invoke(new Action(() =>
            //    {
            //        this.myBook.Items.Insert(0, BookPages[CurrentPage - 1]);
            //        this.myBook.UpdateLayout();
            //    }));
            //}
            //if (CurrentPage - 2 > 0 && this.myBook.Items.Count >= 5)
            //{
            //    this.myBook.Dispatcher.Invoke(new Action(() => {
            //        this.myBook.Items.RemoveAt(this.myBook.Items.Count - 1);
            //        this.myBook.UpdateLayout();
            //    }));
            //}
        }
    }
}
