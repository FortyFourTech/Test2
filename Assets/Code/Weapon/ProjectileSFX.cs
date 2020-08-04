using System.Linq;
using UnityEngine;

public class ProjectileSFX : ProjectileCallbacks
{
    [SerializeField] protected AudioClip[] exitAudio;
    [SerializeField] protected ImpactEffect[] impactEffects;
    [SerializeField] protected ImpactEffect defaultEffect;
    // [SerializeField] AudioSource _source;
    
    protected override void OnFire()
    {
        AudioClip clip = exitAudio.GetRandomElement();
        if (clip)
            AudioSource.PlayClipAtPoint(clip, transform.position);
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

        var clip = matEffect.impactAudio.GetRandomElement();
        if (clip)
            AudioSource.PlayClipAtPoint(clip, hitPoint);
    }

    [System.Serializable]
    public struct ImpactEffect
    {
        public PhysicMaterial hitMat;
        public AudioClip[] impactAudio;
    }
}
