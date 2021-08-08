using System.Windows;
using GKutt.Events;
using Kutt.NET.Links;
using ModernWpf;

namespace GKutt.Controls
{
    public partial class LinkActions
    {
        public static readonly DependencyProperty LinkProperty =
            DependencyProperty.Register(
                "Link", typeof(Link),
                typeof(LinkActions));
        public LinkActions()
        {
            InitializeComponent();
        }

        public Link Link
        {
            get => (Link) GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }

        private void ShowQrCode(object sender, RoutedEventArgs e)
        {
            ActionClicked?.Invoke(this, new ActionClickedEventArgs
            {
                ActionType = ActionType.Qr,
                Data = Link.ShortUrl
            });
        }

        private void DeleteLink(object sender, RoutedEventArgs e)
        {
            ActionClicked?.Invoke(this, new ActionClickedEventArgs
            {
                ActionType = ActionType.Delete,
                Data = Link.Uuid
            });
        }

        public event TypedEventHandler<object, ActionClickedEventArgs> ActionClicked;

    
        private void UpdateLink(object sender, RoutedEventArgs e)
        {
            ActionClicked?.Invoke(this, new ActionClickedEventArgs
            {
                ActionType = ActionType.Edit,
                Data = Link
            });
        }
    }
}