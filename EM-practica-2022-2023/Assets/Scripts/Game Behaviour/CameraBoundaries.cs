using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    // Para asignar los limites de la camara y evitar que se salga del escenario bloqueandola cuando llega a cierto punto
    void Start()
    {
        //A través del codigo, le asignamos los componentes necesarios
        CinemachineConfiner cameraController = FindObjectOfType<CinemachineConfiner>();
        CinemachineVirtualCameraBase virtualCamera = (CinemachineVirtualCameraBase)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
        virtualCamera.PreviousStateIsValid = false;
        cameraController.m_BoundingShape2D = GetComponent<PolygonCollider2D>();
        cameraController.InvalidatePathCache();
    }

   
}
