using Gpm.WebView;
using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseManager : MonoBehaviour
{
    private string databaseURL = "https://novoplextz-default-rtdb.firebaseio.com/Apps.json";
    private bool isFirstRun = true;

    [System.Serializable]
    public class AppData
    {
        public bool IsActive;
        public string URL;
    }

    private void Start()
    {
        isFirstRun = PlayerPrefs.GetInt("IsFirstRun", 1) == 1;

        if (isFirstRun)
        {
            PlayerPrefs.SetInt("IsFirstRun", 0);
            PlayerPrefs.Save();
            return;
        }

        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                LoadActiveApp();
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase: " + dependencyStatus.ToString());
            }
        });
    }

    private void LoadActiveApp()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Apps");
        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to fetch data from Firebase.");
                return;
            }

            DataSnapshot snapshot = task.Result;
            string json = snapshot.GetRawJsonValue();
            Dictionary<string, AppData> appsData = JsonConvert.DeserializeObject<Dictionary<string, AppData>>(json);

            AppData activeApp = null;
            foreach (var kvp in appsData)
            {
                if (kvp.Value.IsActive)
                {
                    activeApp = kvp.Value;
                    break;
                }
            }

            if (activeApp != null)
            {
                ShowUrlFullScreen(activeApp.URL);
            }
            else
            {
                Debug.Log("Active app not found.");
            }
        });
    }

    private void ShowUrlFullScreen(string url)
    {
        GpmWebView.ShowUrl(
            url,
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.FULLSCREEN,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                backgroundColor = "#FFFFFF",
                isNavigationBarVisible = true,
                navigationBarColor = "#4B96E6",
                title = "The page title.",
                isBackButtonVisible = true,
                isForwardButtonVisible = true,
                isCloseButtonVisible = true,
                supportMultipleWindows = true,
                contentMode = GpmWebViewContentMode.MOBILE
            },
            null, null);
    }
}
