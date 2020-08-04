using UnityEngine;
using Dimar.Pools;

public class FirearmVFX : FirearmCallbacks
{
    [SerializeField] protected Transform _exitTransform;
    [SerializeField] protected GameObject _muzzleEffect;

    // protected override void OnFireEmpty() { }

    protected override void OnFire()
    {
        if (_muzzleEffect)
        {
            var vfx = PrefabPooler.Instance.QueueObject(_muzzleEffect, _exitTransform.position, _exitTransform.rotation);
            vfx.ReturnWithDelay(2f);
        }
    }
}
