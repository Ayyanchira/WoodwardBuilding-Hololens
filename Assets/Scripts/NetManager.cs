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

    [Range(.1f,3)] //makes variable a slider in inspector
    public float updateDistance;

    #endregion //Public_Variables

    #region Private_and_Protected
    protected static short messageID = 777; //used in network messaging
    bool firstConnect = false, playerInit = false; //this is not permanent
    
    #endregion //Private_and_Protected

    GameObject vuMarkObject;
    protected GameObject player;

    //message class used for messaging the server, must match what the server is expecting
    public class customMessage : MessageBase 
    { 
        public string deviceType, purpose;
        public Vector3 devicePosition;
        public Quaternion deviceRotation;
    }

    //this function can be deleted, moving StartClient() to replace line 58
    public void connectAsClient()
    {
        StartClient(); //Unity function for creating a network client
    }

    void Start()
    {
        vuMarkObject = GameObject.Find("vuMarks"); //start by find gameobject with "vuMarks" name, save it's gameobject
    }

    void Update() //runs every frame
    {
        if (!firstConnect) //temporary, waiting for the first connect by client
        {
            handler = vuMarkObject.GetComponent<VuMarkHandler>(); //then can check for a VuMarkHandler script on the gameobject
            
            if (handler != null)
            {
                if (handler.childObject != null) //null if image isn't tracked yet
                {
                    firstConnect = true;
                    connectAsClient(); //can be replaced with StartClient()
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

        if(playerInit) //for all location updates after the first vuforia image track
        {
            player = GameObject.FindGameObjectWithTag("Player"); //finding playermovement script on the gameobject
            float posDistance = Vector3.Distance(prevPos, player.transform.position); //distance between last position and now

            if (posDistance > updateDistance) //determined by slider
            {
                prevPos = player.transform.position;

                updateLocation(prevPos, player.transform.rotation); //run message sending function
            }
        }

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn); //base function is run first
        connectionText.text = "Connected";
        connectionText.color = Color.green;
        OnConnected();

        Debug.Log("Connected to server " + conn.address + ". Own ID is: " + conn.connectionId);
        var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity); //spawn player prefab, might need to adjust, gets in the way of arcamera view
        playerInit = true;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        StopClient(); //unity function
    }
   
    public void OnConnected() //network messenger for first tracked location
    {
        var msg = new customMessage();
        msg.devicePosition = handler.childObject.transform.position;
        prevPos = handler.childObject.transform.position;
        msg.deviceType = "Hololens"; //identify self
        msg.purpose = "Initialization"; // purpose of message

        Debug.Log("Device " + msg.deviceType + " has tracked an image at " + msg.devicePosition);

        client.Send(messageID, msg); //actually send, containing an arbitrary id and message class
    }

    public void updateLocation(Vector3 newPosition, Quaternion newRotation) //network messenger for location updates
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
