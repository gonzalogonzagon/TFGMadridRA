using UnityEngine;

public class ExitButtonController : MonoBehaviour
{
    public void ReturnToAndroidApp()
    {        
        #if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        activity.Call("finish");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error communicating with Android: " + e.Message);
            }
        #else
            Debug.Log("Simulated function: Returning to Android app (will only work on real device)");
        #endif
    }
}
