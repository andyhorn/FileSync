using FileSync.ViewModels;
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

namespace FileSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMainWindowViewModel model;
        private Button SelectFilesButton, SyncButton;
        private ListView FileListView;
        public MainWindow()
        {
            model = new MainWindowViewModel();

            InitializeComponent();

            SelectFilesButton = ButtonSelectFiles;
            SyncButton = ButtonSync;
            FileListView = ListViewFileList;

            SelectFilesButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.SelectFiles();
            });

            SyncButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.Sync();
            });

            FileListView.ItemsSource = model.Files;

            DataContext = model;
        }
        
    }
}
