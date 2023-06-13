using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UI;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour
    {
        public GameObject characterPrefab;
        public GameObject timerPrefab;
        

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            InstantiateCharacterServerRpc(OwnerClientId);

            if (!IsOwner) InstantiateTimerServerRpc();
        }
    
        [ServerRpc]
        public void InstantiateCharacterServerRpc(ulong id)
        {
            GameObject characterGameObject = Instantiate(characterPrefab);
            characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            characterGameObject.transform.SetParent(transform, false);
            characterGameObject.GetComponent<PlayerHealth>().Health.Value = 100;

            
        }

        [ServerRpc]
        public void InstantiateTimerServerRpc()
        {
            GameObject timerGameObject = Instantiate(timerPrefab);
        }
    }
}
