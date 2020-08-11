using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter.InformativeViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisconnectedInternetView : StackLayout
    {
        private static StackLayout noficationScatckLayout;

        public DisconnectedInternetView()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            noficationScatckLayout = NotificationPanel;
        }

        public static void SetVisibility(bool isVisible)
        {
            noficationScatckLayout.IsVisible = isVisible;
        }
    }
}