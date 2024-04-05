using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;

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

    void Start()
    {
        isFirstRun = PlayerPrefs.GetInt("IsFirstRun", 1) == 1;

        if (isFirstRun)
        {
            PlayerPrefs.SetInt("IsFirstRun", 0);
            PlayerPrefs.Save();
            return;
        }

        LoadActiveApp();
    }

    public void LoadActiveApp()
    {
        StartCoroutine(GetAppsData());
    }

    IEnumerator GetAppsData()
    {
        UnityWebRequest request = UnityWebRequest.Get(databaseURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            yield break;
        }

        string json = request.downloadHandler.text;

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
            Application.OpenURL(activeApp.URL);
        }
        else
        {
            Debug.Log("Активное приложение не найдено.");
        }
    }
}
