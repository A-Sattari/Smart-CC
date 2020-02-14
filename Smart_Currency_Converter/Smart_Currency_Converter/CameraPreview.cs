/**
 * This thing doesn't work for some reason.
 * https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/view#consuming-the-custom-control
 */

using Xamarin.Forms;

namespace Smart_Currency_Converter
{
        public enum CameraOptions { Rear, Front }
    public sealed class CameraPreview : View
    {

        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
                                                                                    propertyName: "Camera",
                                                                                    returnType: typeof(CameraOptions),
                                                                                    declaringType: typeof(CameraPreview),
                                                                                    defaultValue: CameraOptions.Rear);

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }
    }
}
