using System.Collections;
using System.Collections.Generic;
using Systems;
using UnityEngine;
using Unity.Netcode;
using Movement.Components;
using UnityEngine.SceneManagement;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<int> Health = new NetworkVariable<int>();
    public GameObject healthBar;
    private VictoryConditions victoryCondition;   

    bool isAlive = true;

    void Start()
    {
        victoryCondition = FindObjectOfType<VictoryConditions>(); //Buscamos objeto del tipo VictoryCondition para utilizarlo m�s alante.
    }

    public override void OnNetworkSpawn()
    {
        Health.OnValueChanged += updateHealth; //Delegado que est� comrpobando desde que spawnean los jugadores (vida de estos) su valor vida y lo actualiza para la barra
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += onSceneLoad;
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            // Si es owner, llamo al objeto de tipo victoryDondition para usar el m�todo playerDisconnectedServerRpc y poder actualizar el n�mero de jugdores vivos que quedan
            victoryCondition.playerDisconnectedServerRpc();
            
        }

        if (healthBar != null) //Si la barra de vida est� asignada
        {
            healthBar?.GetComponent<BarraDeVida>()?.CambiarBarra(0); //Ponemos la barra de vida a 0 en caso de desconexi�n
        }
        Health.OnValueChanged -= updateHealth;
    }

    
    //M�todo para cuando recibe algun golpe
    void updateHealth(int previous,int current) 
    {
        if(healthBar != null )  //Si est� asignada
        {
            healthBar?.GetComponent<BarraDeVida>().CambiarBarra(Health.Value); //Actualizamos el valor de la barra de vida 
        }
        
        if (Health.Value <= 0 && (IsServer) && isAlive) //a continuacion comprobamos si esta muerto (su vida ha llegado a 0)
        {
            //En caso de estarlo, eliminamos todo control sobre el personaje y destruimos su sprite ejecutando antes la animaci�n de morir.
            isAlive = false;
            FighterMovement movement = GetComponent<FighterMovement>();
            movement.speed = 0;
            movement.jumpAmount = 0;
            movement.Die();
      
            
            victoryCondition.alivePlayersRemaining.Value -= 1;
            
        }
    }

    //LLamamos a este metodo cuando bajemos la vida
    public void DecreaseHealth(int amount) 
    {
        Health.Value -= amount;
    }

    public int GetVida(){
        return Health.Value;
    }

    //Cuando termina la partida y volvemos al selector, volvemos a generar los personajes borrando el anterior 
    private void onSceneLoad(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
       
        if (sceneName == "SelectorPersonaje")
        {
            if (gameObject != null)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= onSceneLoad;
                Health.OnValueChanged -= updateHealth;
                Destroy(gameObject);
            }
        }
    }

}
