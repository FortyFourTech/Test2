using UnityEngine;

public class MagazineCallbacks : MonoBehaviour
{
    [Tooltip("Will be searched in parents, if not filled")]
    [SerializeField] protected Magazine _magazine;
    
    protected virtual void OnEnable()
    {
        if (!_magazine)
        {
            _magazine = GetComponentInParent<Magazine>();
        }

        if (_magazine)
        {
            _magazine.onRoundsChanged += OnRoundsChanged;
            _magazine.onEmpty += OnEmpty;
            _magazine.onReload += OnReload;
        }
    }

    protected virtual void OnDisable()
    {
        if (_magazine)
        {
            _magazine.onRoundsChanged -= OnRoundsChanged;
            _magazine.onEmpty -= OnEmpty;
            _magazine.onReload -= OnReload;
        }
    }

    protected virtual void OnRoundsChanged() {}
    protected virtual void OnEmpty() {}
    protected virtual void OnReload() {}
}
