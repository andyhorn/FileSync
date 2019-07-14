using System.Windows;

namespace FileSync.Views
{
    /// <summary>
    /// Interaction logic for ProgressBarWindow.xaml
    /// </summary>
    public partial class ProgressBarWindow : Window
    {
        public float Progress
        {
            set
            {
                BarOfProgress.Value = value;
                StatusMessage.Text = $"{value}%";
            }
        }

        public bool IsIndeterminate
        {
            set
            {
                BarOfProgress.IsIndeterminate = value;
            }
        }

        public ProgressBarWindow()
        {
            InitializeComponent();
        }
    }
}
