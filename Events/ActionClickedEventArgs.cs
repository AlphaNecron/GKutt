using System.Windows;

namespace GKutt.Events
{
    public class ActionClickedEventArgs : DependencyObject
    {
        public ActionType ActionType { get; internal set; }

        public object Data { get; internal set; }
    }

    public enum ActionType
    {
        Delete,
        Edit,
        Qr
    }
}