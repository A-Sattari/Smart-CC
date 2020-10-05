using Xamarin.Forms;

namespace Smart_Currency_Converter.Models
{
    public sealed class Currency
    {
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string Symbol { get; set; }
        public ImageSource Flag { get; set; }
    }
}