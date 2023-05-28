using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobby : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI nameMesh;
    [SerializeField] TextMeshProUGUI statusMesh;
    [SerializeField] Image spriteImage;


    public void SetData(string playerName, string playerStatus, Sprite playerImage)
    {
        nameMesh.text = playerName;
        statusMesh.text = playerStatus;
        spriteImage.sprite = playerImage;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
