using System;
using System.Windows;
using System.Windows.Controls;
using GKutt.Pages;
using Kutt.NET.Users;
using ModernWpf.Controls;
using PropertyChanged;
using MessageBox = ModernWpf.MessageBox;

namespace GKutt
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MainWindow
    {
        private UserInfo _userInfo;

        public MainWindow()
        {
            InitializeComponent();
            App.LoadSettings();
            if (string.IsNullOrWhiteSpace(App.ApiKey))
                MessageBox.Show("Please go to Settings and set a valid API key.");
        }

        public string Email { get; private set; } = "";

        private void Navigate(NavigationView s, NavigationViewItemInvokedEventArgs e)
        {
            var container = e.InvokedItemContainer as NavigationViewItem;
            var xaml = (container?.Tag as Type)?.Name;
            if (string.IsNullOrEmpty(xaml)) return;
            RootFrame.Navigate(new Uri($"./Pages/{xaml}.xaml", UriKind.Relative));
        }

        private async void OpenDialog(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!((e.OriginalSource as Button)?.Tag is Type tag)) return;
                var instance = Activator.CreateInstance(tag) as ContentDialog;
                await instance.ShowAsync();
            }
            catch
            {
                // ignored
            }
        }

        private void RefreshList(object sender, RoutedEventArgs e)
        {
            var content = RootFrame.Content;
            if (content is PageManageLink page) page.Refresh();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(App.ApiKey)) return;
                _userInfo = await App.Kutt.GetUserInfoAsync();
                Email = _userInfo.Email;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}