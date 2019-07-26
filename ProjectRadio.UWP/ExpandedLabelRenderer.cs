using ProjectRadio.UWP;
using ProjectRadio.Views.Util;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExpandedLabelRenderer))]
namespace ProjectRadio.UWP
{
    public class ExpandedLabelRenderer : LabelRenderer
    {
        private int _maxLines = 1;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.MaxLines = _maxLines;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (sender is ExtendedLabel)
            {
                _maxLines = (sender as ExtendedLabel).MaxLines;
                if (Control != null)
                {
                    Control.MaxLines = _maxLines;
                }
            }
        }
    }
}