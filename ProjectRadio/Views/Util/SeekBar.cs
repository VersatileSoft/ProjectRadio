using System;
using Xamarin.Forms;

namespace ProjectRadio.Views.Util
{
    public class SeekBar : Slider
    {
        public event EventHandler TouchDown;
        public event EventHandler<SeekBarUpdateArgs> TouchUp;

        public SeekBar()
        {
        }

        public void InvokeTouchDown()
        {
            TouchDown.Invoke(this, EventArgs.Empty);
        }

        public void InvokeTouchUp(double Pos)
        {
            TouchUp.Invoke(this, new SeekBarUpdateArgs() { Position = Pos });
        }
    }

    public class SeekBarUpdateArgs : EventArgs
    {
        public double Position { get; set; }
    }
}