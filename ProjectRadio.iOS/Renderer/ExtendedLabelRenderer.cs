using ProjectRadio.iOS.Renderer;
using ProjectRadio.Views.Util;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace ProjectRadio.iOS.Renderer
{
    public class ExtendedLabelRenderer : LabelRenderer
    {
        private nint _maxLines = 1;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Lines = _maxLines;
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
                    Control.Lines = _maxLines;
                }
            }
        }
    }
}