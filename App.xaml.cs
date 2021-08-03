using System;
using System.IO;
using Kutt.NET;
using ModernWpf;
using Newtonsoft.Json;
using static System.Convert;
using static System.Text.Encoding;

namespace GKutt
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        internal static string ApiKey = "";
        internal static KuttApi Kutt = new(ApiKey);

        private static readonly string ConfigFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gkutt.conf");

        private static string Encrypt(string text)
        {
            return ToBase64String(UTF8.GetBytes(text));
        }

        private static string Decrypt(string encrypted)
        {
            return UTF8.GetString(FromBase64String(encrypted));
        }

        public static string LoadSettings()
        {
            if (!File.Exists(ConfigFile)) return null;
            var def = new {apiKey = ""};
            var raw = File.ReadAllText(ConfigFile);
            try
            {
                var deserialized = JsonConvert.DeserializeAnonymousType(raw, def);
                ApiKey = Decrypt(deserialized?.apiKey);
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
            var apiKey = Encrypt(key);
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