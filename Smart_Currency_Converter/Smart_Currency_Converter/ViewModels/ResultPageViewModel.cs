using System;
using Xamarin.Forms;
using System.Reflection;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

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

    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
                return null;

            ImageSource imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
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
                    RandomName = "30000000000000000000000000000.95"
                },
                new Product()
                {
                    ProductName = "four",
                    RandomName = "4"
                },
                new Product()
                {
                    ProductName = "five",
                    RandomName = "54.70"
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