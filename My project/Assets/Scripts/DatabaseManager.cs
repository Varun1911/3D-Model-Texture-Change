using Firebase.Database;
using UnityEngine;
using Firebase;
using System.Net.NetworkInformation;

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
}
