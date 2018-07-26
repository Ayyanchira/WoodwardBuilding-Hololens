using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VuMarkHandler : MonoBehaviour {

    public string trackedChild;
    public GameObject childObject;

    // Use this for initialization
	void Start ()
    {
        trackedChild = "";
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(trackedChild != "") //check if trackedChild is null
        {
            childObject = GameObject.Find(trackedChild); //if not, find the instantiated game object, check DefaultTrackableEventHandler
        }
	}
}
