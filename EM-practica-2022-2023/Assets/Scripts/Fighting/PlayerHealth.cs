using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    NetworkVariable<int> Health;
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
    }

    public void DecreaseHealth(int amount) 
    {
        Health.Value -= amount;
    }
}
