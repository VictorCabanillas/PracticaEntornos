using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UI;

namespace Netcode
{
    public class PlayerNetworkConfig : NetworkBehaviour
    {
        public GameObject characterPrefab;
        [SerializeField] private VictoryConditions victoryConditions;
        

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            InstantiateCharacterServerRpc(OwnerClientId); 
            victoryConditions = FindObjectOfType<VictoryConditions>(); 
            Debug.Log("hola"); 
            victoryConditions.AddPlayerObject(this.gameObject);
        }

    
        [ServerRpc]
        public void InstantiateCharacterServerRpc(ulong id)
        {
            GameObject characterGameObject = Instantiate(characterPrefab);
            characterGameObject.GetComponent<NetworkObject>().SpawnWithOwnership(id);
            characterGameObject.transform.SetParent(transform, false);
            characterGameObject.GetComponent<PlayerHealth>().Health.Value = 100;
        }
    }
}
