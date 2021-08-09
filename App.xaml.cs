using System;
using System.IO;
using Kutt.NET;
using ModernWpf;
using Newtonsoft.Json;
namespace GKutt
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        internal static string ApiKey = "";
        internal static KuttApi Kutt;

        private static readonly string ConfigFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gkutt.conf");

        public static string LoadSettings()
        {
            if (!File.Exists(ConfigFile)) return null;
            var def = new {apiKey = ""};
            var raw = File.ReadAllText(ConfigFile);
            try
            {
                var deserialized = JsonConvert.DeserializeAnonymousType(raw, def);
                ApiKey = Helper.Decrypt(deserialized?.apiKey);
                Kutt = new KuttApi(ApiKey);
                return ApiKey;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return null;
        }

        public static void SaveSettings(string key)
        {
            var apiKey = Helper.Encrypt(key);
            var config = new
            {
                apiKey
            };
            var serialized = JsonConvert.SerializeObject(config);
            try
            {
                File.WriteAllText(ConfigFile, serialized);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}