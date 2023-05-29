using Movement.Components;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Netcode;

namespace Fighting
{
    public class Weapon : NetworkBehaviour
    {

        public Animator effectsPrefab;
        private static readonly int Hit03 = Animator.StringToHash("hit03");

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if(NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost) { 
            GameObject otherObject = collision.gameObject;
            // Debug.Log($"Sword collision with {otherObject.name}");

            Animator effect = Instantiate(effectsPrefab);
            effect.transform.position = collision.GetContact(0).point;
            effect.SetTrigger(Hit03);

            // TODO: Review if this is the best way to do this
            Debug.Log("GOLPE!");
            otherObject.GetComponent<IFighterReceiver>()?.TakeHit();
            otherObject.GetComponent<PlayerHealth>()?.DecreaseHealth(10);
            }
        }
    }
}
