using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        private const string ANALYZING_ANIMATION = "animation-analyze.json";
        private const string PROCESSSING_ANIMATION = "animation-process.json";

        public LoadingPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            LoadingAnimationView.Animation = PROCESSSING_ANIMATION;
            Device.StartTimer(TimeSpan.FromSeconds(3), ChangeAnimation);
        }

        private bool ChangeAnimation()
        {
            LoadingAnimationView.Animation = ANALYZING_ANIMATION;
            return false;
        }
    }
}