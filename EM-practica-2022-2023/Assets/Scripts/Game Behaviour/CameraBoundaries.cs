using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
 
        FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D = GetComponent<PolygonCollider2D>();
    }

   
}
