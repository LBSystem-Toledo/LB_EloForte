using Android.App;
using LB_EloForte.Interface;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(LB_EloForte.Droid.CloseApplication))]
namespace LB_EloForte.Droid
{
    public class CloseApplication : ICloseApplication
    {
        [Obsolete]
        public void closeApplication()
        {
            Activity activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}