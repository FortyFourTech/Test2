using UnityEngine;

public abstract class FirearmCallbacks : MonoBehaviour
{
    [Tooltip("Will be searched in parents, if not filled")]
    [SerializeField] protected Firearm _firearm;

    private void Start()
    {
        if (!_firearm)
        {
            _firearm = GetComponentInParent<Firearm>();
        }

        if (_firearm)
        {
            _firearm.onFire += OnFire;
            _firearm.onFireEmpty += OnFireEmpty;
        }
    }

    protected virtual void OnFireEmpty() {}
    protected virtual void OnFire() {}
}
