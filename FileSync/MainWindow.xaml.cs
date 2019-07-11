using FileSync.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FileSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IMainWindowViewModel model;
        private Button SelectFilesButton, SelectFoldersButton, SyncButton, ClearButton;
        private ListView FileListView;
        private ToggleSwitch SyncAllToggle;
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
