using UnityEngine;

public abstract class ProjectileCallbacks : MonoBehaviour
{
    [Tooltip("Will be searched in parents, if not filled")]
    [SerializeField] protected ProjectileBase _projectile;

    protected virtual void Start()
    {
        if (!_projectile)
        {
            _projectile = GetComponentInParent<ProjectileBase>();
        }

        if (_projectile)
        {
            _projectile.onFire += OnFire;
            _projectile.onHit += OnHit;
        }
    }

    protected virtual void OnFire() {}
    protected virtual void OnHit(Collider hitCollider, Vector3 hitPoint, Vector3 hitNormal, PhysicMaterial hitMaterial) {}
}
