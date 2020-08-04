using UnityEngine;
using Photon.Pun;
using Dimar.Pools;

namespace VReally.Arena.Weapons
{
    public class FirearmShot : FirearmCallbacks
    {
        [SerializeField] private PhotonView _ownerView;

        [Header("Settings")]
        public float bulletDamage;

        [SerializeField] GameObject _bulletPrefab;

        // protected override void OnFireEmpty() { }

        protected override void OnFire()
        {
            var bullet = PrefabPooler.Instance.QueueObject(_bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<ProjectileBase>().Fire(_ownerView.Owner, _firearm, bulletDamage);
            // if (PhotonNetwork.IsMasterClient)
            // {
            //     RaycastHit hit;
            //     if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
            //     {
            //         var hitPlayer = hit.collider.GetComponentInParent<GamePlayerController>();
            //         if (hitPlayer)
            //         {
            //             var playerHealth = hitPlayer.GetComponent<HealthSystem>();
            //             playerHealth.ApplyChange_Server(new SHealthChangeInfo(-bulletDamage));
            //         }
            //     }
            // }
        }
    }
}
