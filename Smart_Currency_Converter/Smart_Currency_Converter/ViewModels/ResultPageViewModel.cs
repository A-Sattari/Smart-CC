using Xamarin.Forms;
using System.Collections.Generic;
using Model.Smart_Currency_Converter;

namespace ViewModel.Result
{
    public class ResultPageViewModel
    {
        /*
        public Command Convert { get; }

        public ResultPageViewModel()
        {
            Convert = new Command(TempMethod);
        }

        private async void TempMethod()
        {
            HashSet<string> set = Cache.Instance.GetAcronyms();

            string baseCurrency = "AUD";
            string targetCurrency = "USD";

            Converter converter = new Converter();
            decimal r = await converter.Convert(25.45M, baseCurrency, targetCurrency);
        }
        */
        public List<Product> Productsss { get; set; }

        public ResultPageViewModel()
        {
            Productsss = new ProductService().GetProductList();
        }
    }

    public class Product
    {
        public string ProductName { get; set; }
        public string RandomName { get; set; }

    }

    public class ProductService
    {
        public List<Product> GetProductList()
        {
            return new List<Product>()
            {
                new Product()
                {
                    ProductName = "One One One One One One One One One One One One One One One One One One One One One One One One One One One One One One One One One One One One",
                    RandomName = "1"
                },
                new Product()
                {
                    ProductName = "two",
                    RandomName = "2"
                },
                new Product()
                {
                    ProductName = "three three three three three three three three three three three three three three three three three three three three three three three three three",
                    RandomName = "3"
                },
                new Product()
                {
                    ProductName = "four",
                    RandomName = "4"
                },
                new Product()
                {
                    ProductName = "five",
                    RandomName = "5"
                }
                ,
                new Product()
                {
                    ProductName = "five",
                    RandomName = "6"
                }
                ,
                new Product()
                {
                    ProductName = "five",
                    RandomName = "7"
                }
                ,
                new Product()
                {
                    ProductName = "five",
                    RandomName = "8"
                }
                ,
                new Product()
                {
                    ProductName = "five",
                    RandomName = "9"
                }
            };
        }
    }
}