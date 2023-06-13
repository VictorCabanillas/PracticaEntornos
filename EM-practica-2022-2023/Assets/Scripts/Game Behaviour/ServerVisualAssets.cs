using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

public class ServerVisualAssets : NetworkBehaviour
{

    //Desactivamos todo lo visual del server

    public override void OnNetworkSpawn()
    {
        if (!IsServer || IsHost) return;


        SpriteRenderer[] spriterendererList = FindObjectsOfType<SpriteRenderer>();
        Image[] imageList = FindObjectsOfType<Image>();
        TextMeshProUGUI[] textList = FindObjectsOfType<TextMeshProUGUI>();
        Tilemap[] tileList = FindObjectsOfType<Tilemap>();
        TilemapRenderer[] tileRendererList = FindObjectsOfType<TilemapRenderer>();

        
        foreach (SpriteRenderer spriterenderer in spriterendererList)
        {
            
            spriterenderer.enabled = false;
        }

        foreach (Image image in imageList)
        {
            
            image.enabled = false;
        }

        foreach (TextMeshProUGUI text in textList)
        {
            
            text.enabled = false;
        }

        foreach (Tilemap tilin in tileList)
        {
            
            tilin.enabled = false;
        }

        foreach (TilemapRenderer tolon in tileRendererList)
        {
            
            tolon.enabled = false;
        }




    }

    
}
