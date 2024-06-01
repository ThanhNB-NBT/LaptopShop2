using System.Globalization;

namespace LaptopShop2.Functions
{
    public class Function
    {
        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string TitleSlugGeneration(string type, string title, int id)
        {
            string sTitle = type + "-" + SlugGenerator.SlugGenerator.GenerateSlug(title) + "-" + id.ToString() + ".html";
            return sTitle;
        }
        public static string FormatCurrency(decimal value)
        {
            var culture = new CultureInfo("vi-VN");
            return string.Format(culture, "{0:N0} đồng", value);
        }
        public static string GenerateOrderCode()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
