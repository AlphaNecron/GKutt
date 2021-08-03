using System.Windows;

namespace GKutt.Dialogs
{
    public partial class DialogSettings
    {
        public DialogSettings()
        {
            InitializeComponent();
            BoxApiKey.Password = App.LoadSettings();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            App.SaveSettings(BoxApiKey.Password);
            App.ApiKey = App.LoadSettings();
            Hide();
        }
    }
}