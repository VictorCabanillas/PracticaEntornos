using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DisableAtack : NetworkBehaviour
{
    //ESTA CLASE NO LA UTILIZAMOS PERO ESTA BIEN TENERLA YA QUE QUERIAMOS CONSEGUIR QUE FUERA EL SERVER QUIEN CALCULARA SI SE HABÍAN PEGADO, AL FINAL LO REALIZA EL CLIENTE TAMBIÉN
    void Start()
    {
        if(NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)  //Si es server o host
        { 
                gameObject.SetActive(true); //Para que solo el servidor pudiese controlar los ataques (ESTA DESACTIVADO)
        }
    }
}
