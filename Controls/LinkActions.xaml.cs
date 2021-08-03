using System.Windows;
using GKutt.Events;
using ModernWpf;

namespace GKutt.Controls
{
    public partial class LinkActions
    {
        public static readonly DependencyProperty UuidProperty =
            DependencyProperty.Register(
                "Uuid", typeof(string),
                typeof(LinkActions),
                new PropertyMetadata("")
            );

        public static readonly DependencyProperty ShortUrlProperty =
            DependencyProperty.Register(
                "ShortUrl",
                typeof(string),
                typeof(LinkActions),
                new PropertyMetadata("")
            );

        public LinkActions()
        {
            InitializeComponent();
        }

        public string Uuid
        {
            get => (string) GetValue(UuidProperty);
            set => SetValue(UuidProperty, value);
        }

        public string ShortUrl
        {
            get => (string) GetValue(ShortUrlProperty);
            set => SetValue(ShortUrlProperty, value);
        }

        private void ShowQrCode(object sender, RoutedEventArgs e)
        {
            ActionClicked?.Invoke(this, new ActionClickedEventArgs
            {
                ActionType = ActionType.Qr,
                Data = ShortUrl
            });
        }

        private void DeleteLink(object sender, RoutedEventArgs e)
        {
            ActionClicked?.Invoke(this, new ActionClickedEventArgs
            {
                ActionType = ActionType.Delete,
                Data = Uuid
            });
        }

        public event TypedEventHandler<object, ActionClickedEventArgs> ActionClicked;


        private void EditLink(object sender, RoutedEventArgs e)
        {
            ActionClicked?.Invoke(this, new ActionClickedEventArgs
            {
                ActionType = ActionType.Edit,
                Data = Uuid
            });
        }
    }
}