using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Kutt.NET;
using MessageBox = ModernWpf.MessageBox;

namespace GKutt.Pages
{
    public partial class PageCreate
    {
        public PageCreate()
        {
            InitializeComponent();
        }

        private KuttApi ApiInstance { get; set; }

        public ObservableCollection<string> Domains { get; } = new();

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            Domains.Clear();
            Domains.Add("kutt.it");
            if (string.IsNullOrWhiteSpace(App.ApiKey)) return;
            var info = await App.Kutt.GetUserInfoAsync();
            info.Domains.Select(x => x.Address).ToList().ForEach(Domains.Add);
        }

        private async void CreateLink(object sender, RoutedEventArgs e)
        {
            try
            {
                var target = TbTarget.Text;
                var slug = TbSlug.Text;
                var domain = DomainSelector.SelectedValue.ToString();
                var password = TbPassword.Password;
                var desc = TbDesc.Text;
                var minutes = TbExpiration.IsEnabled ? (int) Math.Ceiling(TbExpiration.Value) + "m" : "";
                var link = await App.Kutt.CreateLinkAsync(target, domain, desc, minutes, slug, password: password);
                Clipboard.SetText(link.ShortUrl);
            }
            catch (KuttException kex)
            {
                await MessageBox.ShowAsync(kex.Message, "Couldn't create a shortened link!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}