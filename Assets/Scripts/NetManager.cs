using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class NetManager : NetworkManager
{
    public Text connectionText;
    protected static short messageID = 777;

    public class customMessage : MessageBase
    {
        public string deviceType;
    }
    public void connectAsClient()
    {
        StartClient();
    }

    private void Start()
    {
        //Debug.Log(greeting);
        connectAsClient();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        connectionText.text = "Connected";
        connectionText.color = Color.green;

        Debug.Log("Connected to server " + conn.address);
        MessageToServer("HoloLens");
    }

    void MessageToServer(string deviceConnected)
    {
        var sendMSG = new customMessage();
        sendMSG.deviceType = deviceConnected;

        client.Send(messageID, sendMSG);
    }
}
