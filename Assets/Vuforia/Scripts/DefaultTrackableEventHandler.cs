/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBER_VARIABLES
    public Vector3 syncPosition;
    public bool trackedImage;
    public string imageName;
    public GameObject markPos;
    #endregion //PUBLIC_MEMBER_VARIABLES

    #region PRIVATE_MEMBER_VARIABLES
    protected TrackableBehaviour mTrackableBehaviour;
    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);

        trackedImage = false;
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            //Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound(mTrackableBehaviour);
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    protected virtual void OnTrackingFound(TrackableBehaviour trackedObject)
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        syncPosition = markPos.transform.position; //get Vector3 position of vuforia image that was tracked
        imageName = trackedObject.TrackableName;
        Debug.Log("Tracked " + imageName + " at " + syncPosition);
        trackedImage = true;

        GameObject parent = GameObject.Find("vuMarks");
        VuMarkHandler parentScript = parent.GetComponent<VuMarkHandler>();
        parentScript.trackedChild = imageName;

    }

    /*
        woodwardFloor.transform.position = mapPosition;
        woodwardFloor.transform.rotation = mapRotation
        VuforiaBehaviour.Instance.enabled = false;

        client.RegisterHandler(messageID, receiveServerMessage);

        Vector3 trackPosition = transform.position;
        trackPosition[0] += 2;
        MessageToServer(trackPosition);
    */

    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }













/*
    protected void MessageToServer(Vector3 trackPosition)
    {
        var sendMSG = new customMessage();
        sendMSG.devicePosition = trackPosition;
        sendMSG.deviceType = "Hololens";
        sendMSG.firstConnect = false;
        sendMSG.clientID = myID;

        client.Send(messageID, sendMSG);
    }

    protected void receiveServerMessage(NetworkMessage netMsg)
    {
        var msg = netMsg.ReadMessage<serverMessage>();
        this.myID = msg.myID;

        Debug.Log("This devices server ID is: " + myID);
    }
*/
    #endregion // PRIVATE_METHODS
}
