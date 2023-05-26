using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<int> Health = new NetworkVariable<int>();
    public GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        Health.Value = 100;
        Health.OnValueChanged += updateHealth;
    }
    void updateHealth(int previous,int current) 
    {
        Health.Value = current;
        healthBar.GetComponentInChildren<BarraDeVida>().CambiarBarra(Health.Value);
    }

    public void DecreaseHealth(int amount) 
    {
        Health.Value -= amount;
    }
}
