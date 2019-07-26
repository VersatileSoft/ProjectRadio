using Android.Content;
using ProjectRadio.Droid.Renderer;
using ProjectRadio.Views.Util;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace ProjectRadio.Droid.Renderer
{
    public class ExtendedLabelRenderer : LabelRenderer
    {
        private int _maxLines = 1;

        public ExtendedLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            Control?.SetMaxLines(_maxLines);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (sender is ExtendedLabel)
            {
                _maxLines = (sender as ExtendedLabel).MaxLines;
                Control?.SetMaxLines(_maxLines);
            }
        }
    }
}