using ProjectRadio.iOS.Renderer;
using ProjectRadio.Views.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SeekBar), typeof(SeekBarRenderer))]
namespace ProjectRadio.iOS.Renderer
{
    public class SeekBarRenderer : SliderRenderer
    {
        private bool _isSet = false;

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            UIKit.UISlider slider = Control;

            SeekBar element = (SeekBar)Element;

            if (!_isSet)
            {
                slider.TouchDown += (sender, args) =>
                {
                    element.InvokeTouchDown();
                };

                slider.TouchUpInside += (sender, args) =>
                {
                    element.InvokeTouchUp(element.Value);
                };

                slider.TouchUpOutside += (sender, args) =>
                {
                    element.InvokeTouchUp(element.Value);
                };

                _isSet = true;
            }
        }
    }
}