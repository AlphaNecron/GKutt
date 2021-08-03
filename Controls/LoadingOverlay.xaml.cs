using System.Windows;

namespace GKutt.Controls
{
    public partial class LoadingOverlay
    {
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status", typeof(string),
            typeof(LoadingOverlay),
            new PropertyMetadata(""));

        public LoadingOverlay()
        {
            InitializeComponent();
        }

        public string Status
        {
            get => (string) GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public void SetStatusAndShow(string status)
        {
            Status = status;
            Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            Status = "";
            Visibility = Visibility.Collapsed;
        }
    }
}