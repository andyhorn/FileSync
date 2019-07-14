using FileSync.Models;
using FileSync.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        private delegate void UpdateProgress(float value);

        private void UpdateProgressValue(float value)
        {
            ProgressBarSync.Value = value;
            //model.Progress = value;
        }

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

            model = new MainWindowViewModel();

            FileListView.ItemsSource = model.Files;

            DataContext = model;

            SelectFilesButton.Click += new RoutedEventHandler((sender, target) =>
            {
                var worker = new BackgroundWorker();

                worker.DoWork += delegate (object a, DoWorkEventArgs args)
                {
                    model.SelectFiles();
                };

                worker.RunWorkerCompleted += delegate (object b, RunWorkerCompletedEventArgs args)
                {
                    FileListView.ItemsSource = null;
                    FileListView.ItemsSource = model.Files;

                    model.StatusMessage = $"{model.Files.Count} files selected";
                    ProgressBar.IsIndeterminate = false;
                    SetSyncButton();
                };

                ProgressBar.IsIndeterminate = true;
                worker.RunWorkerAsync();
            });

            SelectFoldersButton.Click += new RoutedEventHandler((sender, target) =>
            {
                var worker = new BackgroundWorker();

                var dispatcher = ProgressBar.Dispatcher;

                var update = new UpdateProgress(UpdateProgressValue);

                worker.DoWork += delegate (object a, DoWorkEventArgs args)
                {
                    Dispatcher.Invoke(() => model.SelectFolders());
                    Dispatcher.Invoke(() => ProgressBar.IsIndeterminate = false);

                    int num = 0, total = model.Directories.Count;

                    var collection = new FileCollection();

                    //UpdateProgress update = new UpdateProgress(UpdateProgressValue);

                    //model.Maximum = total;

                    foreach(var directory in model.Directories)
                    {
                        var files = directory.Files;
                        var subdirs = directory.Directories;

                        collection.AddRange(files);
                        num += 1;

                        float value = (float)num / (float)total;
                        value *= 100;

                        //dispatcher.Invoke(update, value);
                        Dispatcher.Invoke(update, value);
                    }

                    model.Files = collection;
                };

                worker.RunWorkerCompleted += delegate (object b, RunWorkerCompletedEventArgs args)
                {
                    FileListView.ItemsSource = model.Files;
                    SetSyncButton();
                    //var worker2 = new BackgroundWorker();

                    ////var dispatcher = ProgressBar.Dispatcher;

                    //worker2.DoWork += delegate (object c, DoWorkEventArgs args2)
                    //{
                    //    int num = 0, total = model.Directories.Count;

                    //    var collection = new FileCollection();

                    //    //UpdateProgress update = new UpdateProgress(UpdateProgressValue);

                    //    //model.Maximum = total;

                    //    foreach(var directory in model.Directories)
                    //    {
                    //        var files = directory.Files;
                    //        var subdirs = directory.Directories;

                    //        collection.AddRange(files);
                    //        num += 1;

                    //        float value = (float)num / (float)total;
                    //        value *= 100;

                    //        dispatcher.Invoke(update, value);
                    //    }

                    //    model.Files = collection;
                    //};

                    //worker2.RunWorkerCompleted += delegate (object d, RunWorkerCompletedEventArgs args2)
                    //{
                    //    FileListView.ItemsSource = model.Files;
                    //    model.StatusMessage = $"{model.Files.Count} files selected";
                    //    SetSyncButton();
                    //};


                    //ProgressBar.IsIndeterminate = false;
                    //worker2.RunWorkerAsync();

                    //int num = 0, total = model.Directories.Count;

                    //var collection = new FileCollection();

                    //foreach(var directory in model.Directories)
                    //{
                    //    var files = directory.Files;
                    //    var subdirs = directory.Directories;

                    //    collection.AddRange(files);
                    //    num += 1;

                    //    Dispatcher.Invoke(() => ProgressBar.Value = (double)(((decimal)num / (decimal)total) * 100));
                    //}

                    //model.Files = collection;
                    //FileListView.ItemsSource = model.Files;
                    //model.StatusMessage = $"{model.Files.Count} files selected";
                    //SetSyncButton();
                };

                ProgressBar.IsIndeterminate = true;
                worker.RunWorkerAsync();
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
