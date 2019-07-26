using Android.Content;
using Android.Widget;
using ProjectRadio.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ProjectRadio.Views.Util.SeekBar), typeof(SeekBarRenderer))]
namespace ProjectRadio.Droid.Renderer
{
    public class SeekBarRenderer : SliderRenderer
    {

        public SeekBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            Views.Util.SeekBar element = (ProjectRadio.Views.Util.SeekBar)Element;
            if (Control != null)
            {
                SeekBar seekBar = Control;

                seekBar.StartTrackingTouch += (sender, args) =>
                {
                    element.InvokeTouchDown();
                };

                seekBar.StopTrackingTouch += (sender, args) =>
                {
                    element.InvokeTouchUp(element.Value);
                };

                seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs args) =>
                {
                    if (args.FromUser)
                    {
                        element.Value = (element.Minimum + ((element.Maximum - element.Minimum) * (args.Progress) / 1000.0));
                    }
                };
            }
        }
    }
}