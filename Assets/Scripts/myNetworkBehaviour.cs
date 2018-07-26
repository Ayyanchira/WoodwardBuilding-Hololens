using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class myNetworkBehaviour : NetworkBehaviour {
    //this class useless

	[Command]
    public void CmdnewPosition()
    {
        Debug.Log("Hello from the client");
    }

    void Update()
    {
        
    }    
}
