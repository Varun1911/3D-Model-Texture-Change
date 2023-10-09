using Firebase.Database;
using UnityEngine;
using Firebase;
using System.Net.NetworkInformation;
using Firebase.Extensions;

public class DatabaseManager : MonoBehaviour
{
    private DatabaseReference reference;
    private string UserMacAddress;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        UserMacAddress = GetMacAddress();
    }


    void Update()
    {


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
        reference.Child("Users").Child("User1").Child("MacAddress").SetValueAsync(UserMacAddress);
    }


    public void LoadData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWithOnMainThread(task => {
          if (task.IsFaulted)
          {
              // Handle the error...
          }
          else if (task.IsCompleted)
          {
                DataSnapshot snapshot = task.Result;
                snapshot.Child("User1").Child("MacAddress").GetValue(true);
              // Do something with snapshot...
          }
      });
    }
}
