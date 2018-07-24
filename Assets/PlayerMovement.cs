using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
    public class customMessage : MessageBase
    {
        public string deviceType;
        public Vector3 devicePosition;
    }

    // public GameObject ARCamera;
    GameObject spawnedPlayer;
    GameObject ARCamera;
    NetworkIdentity ownID;
    Vector3 currentPos;
    

    // Use this for initialization
    void Start()
    {
        spawnedPlayer = GameObject.FindGameObjectWithTag("Player"); //Find player prefab
        ARCamera = GameObject.FindGameObjectWithTag("MainCamera"); //Find camera object
    }

    // Update is called once per frame
    void Update()
    {
        spawnedPlayer.transform.position = ARCamera.transform.position;
        currentPos = spawnedPlayer.transform.position;
        spawnedPlayer.transform.rotation = ARCamera.transform.rotation;



    }
}
