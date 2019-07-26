using ProjectRadio.UWP;
using ProjectRadio.Views.Util;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(SeekBar), typeof(SeekBarRenderer))]
namespace ProjectRadio.UWP
{
    public class SeekBarRenderer : SliderRenderer
    {
        private bool _isSet = false;

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            FormsSlider slider = Control;

            SeekBar element = (SeekBar)Element;

            if (!_isSet)
            {
                slider.IsThumbToolTipEnabled = false;

                slider.AddHandler(PointerPressedEvent, new PointerEventHandler((sender, args) =>
                {
                    element.InvokeTouchDown();
                }), true);

                slider.AddHandler(PointerReleasedEvent, new PointerEventHandler((sender, args) =>
                {
                    element.InvokeTouchUp(element.Value);
                }), true);

                _isSet = true;
            }
        }
    }
}