using ProjectRadio.iOS.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace ProjectRadio.iOS.Renderer
{
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.CornerRadius = 5;
                Control.Layer.BorderColor = Color.LightGray.ToCGColor();
                Control.Layer.BorderWidth = 1;
            }
        }
    }
}