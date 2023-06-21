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

    //Para cambiar las barras del selector (DESUSO)
    public void SetData(string playerName, string playerStatus, Sprite playerImage)
    {
        nameMesh.text = playerName;
        statusMesh.text = playerStatus;
        spriteImage.sprite = playerImage;
    }

    
}
