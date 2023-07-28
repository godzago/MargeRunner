using UnityEngine;

namespace HyperCasualRunner
{
    /// <summary>
    /// It sets the frame rate to the maximum refresh rate of the device's screen. You don't need to place it in the scene in order this to work.
    /// </summary>
    public static class FrameRateSetter
    {
        [RuntimeInitializeOnLoadMethod]
        static void SetFrameRate()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }
    }
}