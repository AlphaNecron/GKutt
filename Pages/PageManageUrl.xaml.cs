using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GKutt.Events;
using Kutt.NET;
using Kutt.NET.Links;
using ModernWpf.Controls;
using PropertyChanged;
using QRCoder;
using MessageBox = ModernWpf.MessageBox;

namespace GKutt.Pages
{
    [AddINotifyPropertyChangedInterface]
    public partial class PageManageUrl
    {
        public PageManageUrl()
        {
            InitializeComponent();
            var view = (CollectionView) CollectionViewSource.GetDefaultView(LvLinks.ItemsSource);
            view.Filter = LinkFilter;
        }

        public ObservableCollection<string> Queries { get; } = new();
        public ObservableCollection<Link> Links { get; } = new();

        private bool LinkFilter(object item)
        {
            if (string.IsNullOrEmpty(SbQuery.Text))
            {
                return true;
            }

            var link = item as Link;
            return link.LongUrl.IndexOf(SbQuery.Text, StringComparison.OrdinalIgnoreCase) >= 0
                   || link.Address.IndexOf(SbQuery.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        internal async void Refresh(object sender = null, RoutedEventArgs e = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(App.ApiKey)) return;
                Overlay.SetStatusAndShow("Getting your links...");
                var operation = await App.Kutt.GetListOfLinksAsync(1000, all: true);
                Links.Clear();
                operation.Links.ToList().ForEach(link =>
                {
                    Links.Add(link);
                    Queries.Add(link.Address);
                    Queries.Add(link.LongUrl);
                });
            }
            catch (KuttException kex)
            {
                await MessageBox.ShowAsync(kex.Message, "Couldn't finish your request.", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                Overlay.Hide();
            }
        }

        private async void DeleteLink(string uuid)
        {
            try
            {
                var result = await MessageBox.ShowAsync("Are you sure you want to delete this link?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result is MessageBoxResult.Yes)
                {
                    Overlay.SetStatusAndShow("Deleting selected link...");
                    await App.Kutt.DeleteLinkAsync(uuid);
                }

                Refresh();
            }
            catch (KuttException kex)
            {
                await MessageBox.ShowAsync(kex.Message, "Couldn't finish your request.", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                Overlay.Hide();
            }
        }

        private void HandleActions(object sender, ActionClickedEventArgs e)
        {
            var type = e.ActionType;
            switch (type)
            {
                case ActionType.Delete:
                    DeleteLink(e.Data.ToString());
                    break;
                case ActionType.Edit:
                    break;
                case ActionType.Qr:
                    ShowQr(e.Data.ToString());
                    break;
            }
        }

        private async void ShowQr(string link)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new XamlQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);
            var dialog = new ContentDialog
            {
                Content = new Image
                {
                    Width = 225,
                    Height = 225,
                    Source = qrCodeImage
                }
            };
            try
            {
                await dialog.ShowAsync();
            }
            catch
            {
                // ignored
            }
        }

        private void QueryChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            CollectionViewSource.GetDefaultView(LvLinks.ItemsSource).Refresh();
        }

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
            var src = e.Source as Button;
            if (src?.Tag is string content && !string.IsNullOrWhiteSpace(content))
                Clipboard.SetText(content);
        }
    }
}