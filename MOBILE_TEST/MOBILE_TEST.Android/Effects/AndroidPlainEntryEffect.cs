using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MOBILE_TEST.Droid.Effects;
using MOBILE_TEST.Effects; // 네 네임스페이스에 맞춰 수정
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MOBILE_TEST")]
[assembly: ExportEffect(typeof(AndroidPlainEntryEffect), "PlainEntry")]

namespace MOBILE_TEST.Droid.Effects
{
    public class AndroidPlainEntryEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                if (Control != null)
                {
                    Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                    Control.Background = null;
                }
            }
            catch { }
        }

        protected override void OnDetached()
        {
        }
    }
}