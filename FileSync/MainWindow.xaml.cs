using FileSync.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private IMainWindowViewModel model;
        private Button SelectFilesButton, SelectFoldersButton, SyncButton, ClearButton;
        private ListView FileListView;
        private ToggleSwitch SyncAllToggle;

        private void TextBlock_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var link = sender as TextBlock;
            link.TextDecorations = TextDecorations.Underline;
        }

        private void TextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var link = sender as TextBlock;
            link.TextDecorations = TextDecorations.Baseline;
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = sender as TextBlock;
            var tag = item.Tag.ToString();

            var file = model.Files.FirstOrDefault(x => x.FullName == tag);

            model.Files.Remove(file);
        }

        private ProgressBar ProgressBar;
        public MainWindow()
        {
            InitializeComponent();

            SelectFilesButton = ButtonSelectFiles;
            SelectFoldersButton = ButtonSelectFolders;
            SyncButton = ButtonSync;
            ClearButton = ButtonClear;
            FileListView = ListViewFileList;
            SyncAllToggle = ToggleSwitchSyncAll;
            ProgressBar = ProgressBarSync;

            model = new MainWindowViewModel(ProgressBar);

            FileListView.ItemsSource = model.Files;

            DataContext = model;

            SelectFilesButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.SelectFiles();
                SetSyncButton();
            });

            SelectFoldersButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.SelectFolders();
                SetSyncButton();
            });

            SyncButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.Sync();
            });

            ClearButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.Clear();
                SetSyncButton();
            });

            SyncAllToggle.IsCheckedChanged += new EventHandler((sender, target) =>
            {
                model.SyncAll = (bool)(sender as ToggleSwitch).IsChecked;
            });
        }

        private void SetSyncButton()
        {
            SyncButton.IsEnabled = model.Files.Count > 0;
        }
    }
}
