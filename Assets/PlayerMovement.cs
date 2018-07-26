using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour
{
    GameObject spawnedPlayer;
    GameObject ARCamera;
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
        currentPos = spawnedPlayer.transform.position; //saving it for use in another script
        spawnedPlayer.transform.rotation = ARCamera.transform.rotation;
    }
}
