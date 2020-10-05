using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Currency_Converter.InformativeViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorPromptView : StackLayout
    {
        private static StackLayout errorPromptPanel;
        private static Label messageLabel;
        
        public ErrorPromptView()
        {
            InitializeComponent();
            ErrorPromptPanel.IsVisible = false;
            errorPromptPanel = ErrorPromptPanel;
            messageLabel = MessageLabel;
        }

        public static void Display(string message, bool autoHide = true)
        {
            if (!errorPromptPanel.IsVisible)
            {
                messageLabel.Text = message;
                errorPromptPanel.IsVisible = true;

                if (autoHide)
                {
                    Device.StartTimer(TimeSpan.FromSeconds(7),
                                      () => errorPromptPanel.IsVisible = false);
                }
            }
        }
    }
}