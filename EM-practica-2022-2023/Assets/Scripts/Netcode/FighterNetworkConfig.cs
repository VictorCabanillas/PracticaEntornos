﻿using Cinemachine;
using Movement.Components;
using Systems;
using Unity.Netcode;
using UnityEngine;

namespace Netcode
{
    public class FighterNetworkConfig : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            
            FighterMovement fighterMovement = GetComponent<FighterMovement>();
            InputSystem.Instance.Character = fighterMovement;
            ICinemachineCamera virtualCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera;
            Debug.Log(virtualCamera);
            virtualCamera.Follow = transform;
        }
    }
}