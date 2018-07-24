using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class NetManager : NetworkManager
{
    #region Public_Variables
    public Text connectionText;
    public VuMarkHandler handler;
    public Vector3 prevPos;
    public float updateDistance;
    #endregion //Public_Variables

    #region Private_and_Protected
    protected static short messageID = 777;
    bool firstConnect = false, playerInit = false; //this is not permanent
    
    #endregion //Private_and_Protected

    GameObject vuMarkObject;
    protected GameObject player;

    public class customMessage : MessageBase
    { 
        public string deviceType, purpose;
        public Vector3 devicePosition;
        public Quaternion deviceRotation;
    }

    public void connectAsClient()
    {
        StartClient();
    }

    void Start()
    {
        vuMarkObject = GameObject.Find("vuMarks");
    }

    void Update()
    {
        if (!firstConnect)
        {
            handler = vuMarkObject.GetComponent<VuMarkHandler>();
            
            if (handler != null)
            {
                if (handler.childObject != null)
                {
                    firstConnect = true;
                    connectAsClient();
                }
           
                else
                {
                    Debug.Log("Image not tracked yet");
                }
            }
            else
            {
                Debug.Log("Handler not found...");
            }
        }

        if(playerInit)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            float posDistance = Vector3.Distance(prevPos, player.transform.position);

            if (posDistance > updateDistance)
            {
                prevPos = player.transform.position;

                updateLocation(prevPos, player.transform.rotation);
            }
        }

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        connectionText.text = "Connected";
        connectionText.color = Color.green;
        OnConnected();

        Debug.Log("Connected to server " + conn.address + ". Own ID is: " + conn.connectionId);
        var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerInit = true;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        StopClient();
    }
   
    public void OnConnected()
    {
        var msg = new customMessage();
        msg.devicePosition = handler.childObject.transform.position;
        prevPos = handler.childObject.transform.position;
        msg.deviceType = "Hololens";
        msg.purpose = "Initialization";

        Debug.Log("Device " + msg.deviceType + " has tracked an image at " + msg.devicePosition);

        client.Send(messageID, msg);
    }

    public void updateLocation(Vector3 newPosition, Quaternion newRotation)
    {
        var msg = new customMessage();
        msg.devicePosition = newPosition;
        msg.deviceType = "Hololens";
        msg.purpose = "Synchronization";
        msg.deviceRotation = newRotation;

        Debug.Log("Device " + msg.deviceType + " at position " + msg.devicePosition);

        client.Send(messageID, msg);
    }
}
