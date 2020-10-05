using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter.InformativeViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        private const string ANALYZING_ANIMATION = "animation-analyze.json";
        private const string PROCESSSING_ANIMATION = "animation-process.json";

        public LoadingPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            LoadingAnimationView.Animation = PROCESSSING_ANIMATION;
            LoadingStatus.Text = "Processing ...";
            Device.StartTimer(TimeSpan.FromSeconds(4), ChangeAnimation);
        }

        protected override bool OnBackButtonPressed() => false;

        private bool ChangeAnimation()
        {
            LoadingAnimationView.Animation = ANALYZING_ANIMATION;
            LoadingStatus.Text = "Analyzing Image";
            return false;
        }
    }
}