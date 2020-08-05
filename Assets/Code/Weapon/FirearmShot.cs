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
        }
    }
}
