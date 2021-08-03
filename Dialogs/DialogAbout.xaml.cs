using System.Reflection;

namespace GKutt.Dialogs
{
    public partial class DialogAbout
    {
        public DialogAbout()
        {
            InitializeComponent();
        }

        public string AppVersion
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                return $"v{version.Major}.{version.Minor}.{version.Build}";
            }
        }
    }
}