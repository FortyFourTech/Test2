using System.Linq;
using UnityEngine;
using Dimar.Pools;

public class ProjectileVFX : ProjectileCallbacks
{
    [SerializeField] protected GameObject exitParticle;
    [SerializeField] protected ImpactEffect[] impactEffects;
    [SerializeField] protected ImpactEffect defaultEffect;
    
    protected override void OnFire()
    {
        if (exitParticle)
        {
            var vfx = PrefabPooler.Instance.QueueObject(exitParticle, transform.position, transform.rotation);
            vfx.ReturnWithDelay(2f);
        }
    }

    protected override void OnHit(Vector3 hitPoint, Vector3 hitNormal, PhysicMaterial hitMaterial)
    {
        var materialEffects = impactEffects.Where(eff => eff.hitMat == hitMaterial);

        ImpactEffect matEffect;
        if (materialEffects.Count() > 0)
        {
            matEffect = materialEffects.First();
        }
        else
        {
            matEffect = defaultEffect;
        }

        var vfx = PrefabPooler.Instance.QueueObject(matEffect.impactParticle, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitNormal));
        vfx.ReturnWithDelay(2f);
    }

    [System.Serializable]
    public struct ImpactEffect
    {
        public PhysicMaterial hitMat;
        public GameObject impactParticle;
    }
}
