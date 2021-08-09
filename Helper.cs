using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Data;
using static System.Convert;
using static System.Text.Encoding;


namespace GKutt
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class ReverseBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => !(bool) value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => !(bool) value;
    }

    [ValueConversion(typeof(string), typeof(Uri))]
    public class EmailToGravatar : IValueConverter
    {
        private const string Fallback =
            "https://user-images.githubusercontent.com/57827456/123107650-ee4ff380-d463-11eb-89a9-b87323fd28a3.png";

        private const string Gravatar = "https://www.gravatar.com/avatar";
        private const byte Size = 24;
        public static EmailToGravatar Instance => new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hash = Helper.HashEmail(value?.ToString());
            var url = $"{Gravatar}/{hash}?s={Size}&d={Fallback}";
            return new Uri(url);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(string), typeof(bool))]
    public class UrlCheck : IValueConverter
    {
        public static UrlCheck Instance => new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Uri.IsWellFormedUriString(new UriBuilder(value!.ToString()).Uri.ToString(), UriKind.Absolute);
            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Type), typeof(Visibility))]
    public class PageTypeMatcher : IValueConverter
    {
        public static PageTypeMatcher Instance => new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => BoolToVisibility(value as Type == parameter as Type);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static Visibility BoolToVisibility(bool @bool)
        {
            return @bool ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToYesNo : IValueConverter
    {
        public static BooleanToYesNo Instance => new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => (bool) value! ? "Yes" : "No";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    
    public class EmptyStringChecker : IValueConverter
    {
        public static EmptyStringChecker Instance => new();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => parameter as string == "visibility" ? string.IsNullOrWhiteSpace((string) value) ? Visibility.Hidden : Visibility.Visible : string.IsNullOrWhiteSpace((string) value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class Helper
    {
        internal static string Encrypt(string text) 
            => ToBase64String(UTF8.GetBytes(text));

        internal static string Decrypt(string encrypted) 
            => UTF8.GetString(FromBase64String(encrypted));

        public static string HashEmail(string email)
        {
            var e = email.Trim().ToLower();
            using var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(e));
            var sBuilder = new StringBuilder();
            foreach (var t in data) sBuilder.Append(t.ToString("x2"));
            return sBuilder.ToString();
        }
    }
}