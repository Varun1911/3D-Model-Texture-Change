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


// The Database will be saved as 
//   User
//   |- {userMacAddress}
//       |- macAddress
//       |- textures
//           |- cube1
//           |- cube2
//           |- ...

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference reference;
    private string userMacAddress;

    private UserData data;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                reference = FirebaseDatabase.DefaultInstance.RootReference;

                // Initialize userMacAddress here
                userMacAddress = GetMacAddress();

                // Load user data
                StartCoroutine(LoadUserData());
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + task.Exception);
            }
        });
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
        DatabaseReference userRef = reference.Child("User").Child(userMacAddress);
        string json = JsonUtility.ToJson(data);
        userRef.SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Failed to save user data to Firebase: " + task.Exception);
            }
            else
            {
                Debug.Log("User data saved successfully.");
            }
        });
    }

    private IEnumerator LoadUserData()
    {
        // Wait until Firebase is initialized
        yield return new WaitUntil(() => reference != null);

        // Read data for the specified user
        DatabaseReference userRef = reference.Child("User").Child(userMacAddress);

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
                data = JsonUtility.FromJson<UserData>(jsonData);
            }
            else
            {
                // If data doesn't exist in the database, create a new UserData object
                data = new UserData();
            }
        });
    }
}