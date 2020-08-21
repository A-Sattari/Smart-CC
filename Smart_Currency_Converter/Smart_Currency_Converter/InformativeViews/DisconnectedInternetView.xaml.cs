using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter.InformativeViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisconnectedInternetView : StackLayout
    {
        private static StackLayout errorNotification;

        public static bool Visibility
        {
            //get => errorNotification;
            set => errorNotification.IsVisible = value;
        }

        public DisconnectedInternetView()
        {
            InitializeComponent();
            ErrorNotificationPanel.IsVisible = false;
            errorNotification = ErrorNotificationPanel;
        }

        //public static void SetVisibility(bool isVisible)
        //{
        //    errorNotification.IsVisible = isVisible;
        //}
    }
}