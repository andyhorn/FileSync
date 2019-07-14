using FileSync.Models;
using FileSync.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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
        private Slider SyncSlider;
        private Views.ProgressBarWindow _progress;
        private Dispatcher _dispatcher;

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
            model.FilesChanged();
        }

        private delegate void UpdateProgress(float value);

        private void UpdateProgressValue(float value)
        {
            _progress.Progress = value;
        }

        private Overwrite GetSyncValue(double value)
        {
            switch(value)
            {
                case 0:
                    return Overwrite.None;
                case 1:
                    return Overwrite.New;
                case 2:
                    return Overwrite.All;
                default:
                    throw new ArgumentOutOfRangeException("Overwrite");
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            SelectFilesButton = ButtonSelectFiles;
            SelectFoldersButton = ButtonSelectFolders;
            SyncButton = ButtonSync;
            ClearButton = ButtonClear;
            FileListView = ListViewFileList;
            SyncSlider = SyncOptionSlider;

            model = new MainWindowViewModel
            {
                Overwrite = GetSyncValue(SyncSlider.Value)
            };

            FileListView.ItemsSource = model.Files;

            DataContext = model;

            SelectFilesButton.Click += new RoutedEventHandler((sender, target) =>
            {
                var worker = new BackgroundWorker();

                _progress = new Views.ProgressBarWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                worker.DoWork += delegate (object a, DoWorkEventArgs args)
                {
                    model.SelectFiles();
                };

                worker.RunWorkerCompleted += delegate (object b, RunWorkerCompletedEventArgs args)
                {
                    FileListView.ItemsSource = null;
                    FileListView.ItemsSource = model.Files;

                    model.StatusMessage = $"Done!";
                    _progress.IsIndeterminate = false;
                    _progress.Close();

                    //FileCountMessageBox.Text = $"{model.Files.Count} files selected";
                };

                _progress.IsIndeterminate = true;
                _progress.Show();
                worker.RunWorkerAsync();
            });

            SelectFoldersButton.Click += new RoutedEventHandler((sender, target) =>
            {
                var worker = new BackgroundWorker();

                _progress = new Views.ProgressBarWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                _dispatcher = _progress.Dispatcher;

                var update = new UpdateProgress(UpdateProgressValue);

                worker.DoWork += delegate (object a, DoWorkEventArgs args)
                {
                    Dispatcher.Invoke(() => model.SelectFolders());
                    //Dispatcher.Invoke(() => _progress.IsIndeterminate = false);

                    //int num = 0, total = model.Directories.Count;

                    //var collection = new FileCollection();

                    //foreach(var directory in model.Directories)
                    //{
                    //    var files = directory.Files;
                    //    var subdirs = Directory.GetAllContents(directory);

                    //    collection.AddRange(files);
                    //    num += 1;

                    //    float value = num / (float)total;
                    //    value *= 100;

                    //    _dispatcher.Invoke(update, value);
                    //}

                    //model.Files = collection;
                };

                worker.RunWorkerCompleted += delegate (object b, RunWorkerCompletedEventArgs args)
                {
                    FileListView.ItemsSource = null;
                    FileListView.ItemsSource = model.Files;
                    model.StatusMessage = "Files scanned; ready to sync.";
                    _progress.Close();
                };

                _progress.IsIndeterminate = true;
                _progress.Show();
                worker.RunWorkerAsync();
            });

            SyncButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.Sync();
            });

            ClearButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.Clear();
            });

            SyncOptionSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>((sender, target) =>
            {
                var value = (sender as Slider).Value;

                model.Overwrite = GetSyncValue(value);
            });
        }
    }
}
