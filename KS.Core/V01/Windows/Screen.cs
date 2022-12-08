using GI.Screenshot;

namespace KS.Core.V01.Windows;

public static class Screen
{
    public static void ScreenShoot()
    {
        var image = Screenshot.CaptureRegion();

// get a screenshot of given region
        //var image = Screenshot.CaptureRegion(rect);

// get a screenshot of all screens
        var image = Screenshot.CaptureAllScreens();
    }
}