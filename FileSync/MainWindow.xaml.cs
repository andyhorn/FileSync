using FileSync.Models;
using FileSync.ViewModels;
using MahApps.Metro.Controls;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IMainWindowViewModel model;
        private Button SelectFilesButton, SyncButton;
        private ListView FileListView;
        //private ComboBox SyncOptionsComboBox;
        private ToggleSwitch SyncAllToggle;
        private ProgressBar ProgressBar;
        public MainWindow()
        {
            InitializeComponent();

            SelectFilesButton = ButtonSelectFiles;
            SyncButton = ButtonSync;
            FileListView = ListViewFileList;
            SyncAllToggle = ToggleSwitchSyncAll;
            //SyncOptionsComboBox = ComboBoxSyncOptions;
            ProgressBar = ProgressBarSync;

            model = new MainWindowViewModel(ProgressBar);

            //SyncOptionsComboBox.SelectedItem = model.SelectedOption;

            FileListView.ItemsSource = model.Files;

            DataContext = model;

            SelectFilesButton.Click += new RoutedEventHandler((sender, target) =>
            {
                model.SelectFiles();
            });

            SyncButton.Click += new RoutedEventHandler((sender, target) =>
            {                
                model.Sync();
            });

            SyncAllToggle.IsCheckedChanged += new EventHandler((sender, target) =>
            {
                model.SyncAll = (bool)(sender as ToggleSwitch).IsChecked;
            });

            //SyncOptionsComboBox.SelectionChanged += new SelectionChangedEventHandler((sender, target) =>
            //{
            //    model.SelectedOption = SyncOptionsComboBox.SelectedItem as SyncOption;
            //});
        }
        
    }
}
