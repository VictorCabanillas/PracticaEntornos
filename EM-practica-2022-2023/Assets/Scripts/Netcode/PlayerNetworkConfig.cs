using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UI;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour
    {
        public GameObject characterPrefab;
        UiManager UImanager;
        GameObject healthBar;

        private void Awake()
        {
            UImanager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UiManager>();
        }

        public override void OnNetworkSpawn()
        {
            healthBar=UImanager.CrearBarras((int)OwnerClientId);
            //Debug.Log((int)OwnerClientId);
            if (!IsOwner) return;
            InstantiateCharacterServerRpc(OwnerClientId);
        }

    
        [ServerRpc]
        public void InstantiateCharacterServerRpc(ulong id)
        {
            GameObject characterGameObject = Instantiate(characterPrefab);
            characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            characterGameObject.transform.SetParent(transform, false);
            characterGameObject.GetComponent<PlayerHealth>().healthBar = healthBar;
        }
    }
}
