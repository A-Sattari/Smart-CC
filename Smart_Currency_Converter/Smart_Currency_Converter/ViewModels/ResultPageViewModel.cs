using Xamarin.Forms;
using Model.Smart_Currency_Converter;

namespace ViewModel.Result
{
    public class ResultPageViewModel
    {
        public Command Convert { get; }

        public ResultPageViewModel()
        {
            Convert = new Command(TempMethod);
        }

        private void TempMethod()
        {
            Converter converter = new Converter();
            converter.Convert(10, "", "");
        }
    }
}