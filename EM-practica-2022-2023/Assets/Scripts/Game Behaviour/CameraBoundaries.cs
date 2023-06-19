using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CinemachineConfiner cameraController = FindObjectOfType<CinemachineConfiner>();
        CinemachineVirtualCameraBase virtualCamera = (CinemachineVirtualCameraBase)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        virtualCamera.PreviousStateIsValid = false;
        cameraController.m_BoundingShape2D = GetComponent<PolygonCollider2D>();
        cameraController.InvalidatePathCache();
    }

   
}
