using System;
using Kutt.NET.Links;
using ModernWpf;
using ModernWpf.Controls;
using PropertyChanged;

namespace GKutt.Dialogs
{
    [AddINotifyPropertyChangedInterface]
    public partial class DialogUpdateLink
    {
        public string Target { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        
        internal string Uuid;
        public DialogUpdateLink(Link link)
        {
            InitializeComponent();
            Uuid = link.Uuid;
            Target = link.LongUrl;
            Slug = link.Address;
            Description = link.Description;
        }
    }
}