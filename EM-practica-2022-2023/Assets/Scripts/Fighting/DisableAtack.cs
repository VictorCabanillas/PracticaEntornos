using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DisableAtack : NetworkBehaviour
{

    
    void Start()
    {
        if(NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost) 
        { 
                gameObject.SetActive(true);
            
        }
    }
}
