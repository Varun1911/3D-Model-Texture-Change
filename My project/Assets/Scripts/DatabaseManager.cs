using Firebase.Database;
using UnityEngine;
using Firebase;
using System.Net.NetworkInformation;
using Firebase.Extensions;
using System.Collections;

[System.Serializable]
public class UserData
{
    public string macAddress;
    public PartTextures textures;
}

[System.Serializable]
public class PartTextures
{
    public string cube1;
    public string cube2;
    public string cube3;
    public string cube4;
    public string cube5;
    public string cube6;
    public string cube7;
    public string cube8;
    public string cube9;

    public PartTextures()
    {
        cube1 = "Base";
        cube2 = "Base";
        cube3 = "Base";
        cube4 = "Base";
        cube5 = "Base";
        cube6 = "Base";
        cube7 = "Base";
        cube8 = "Base";
        cube9 = "Base";
    }
}

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference reference;
    private string userMacAddress;

    private UserData data;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });

        userMacAddress = GetMacAddress();

        data = new UserData();
        data.macAddress = userMacAddress;
        data.textures = new PartTextures();
        SaveData();
    }


    string GetMacAddress()
    {
        string result = "";
        foreach (NetworkInterface ninf in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ninf.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
            if (ninf.OperationalStatus == OperationalStatus.Up)
            {
                result += ninf.GetPhysicalAddress().ToString();
                break;
            }
        }
        return result;
    }


    public void SaveData()
    {
        reference.Child("User").Child(userMacAddress);
        reference.SetRawJsonValueAsync(JsonUtility.ToJson(data));
    }


    private IEnumerator LoadUserData()
    {
        // Wait until Firebase is initialized
        yield return new WaitUntil(() => reference != null);

        // Read data for the specified user
        DatabaseReference userRef = reference.Child("user").Child(userMacAddress);

        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to load user data from Firebase: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                // Deserialize the JSON data into your custom class
                string jsonData = snapshot.GetRawJsonValue();
                UserData userData = JsonUtility.FromJson<UserData>(jsonData);

            }
        });
    }
}
