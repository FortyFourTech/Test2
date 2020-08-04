using Photon.Pun;
using UnityEngine;

public abstract class PickupCallbacks : MonoBehaviour
{
    [Tooltip("Will be searched on same GO, if not filled")]
    [SerializeField] protected PickupBase _base;

    private void Start()
    {
        if (!_base)
        {
            _base = GetComponentInParent<PickupBase>();
        }

        if (_base)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _base.onConsumed += OnConsume_Master;
            }
            else
            {
                _base.onConsumed += OnConsume_Slave;
            }
        }
        
    }

    protected abstract void OnConsume_Master(GamePlayerController byPlayer);
    protected virtual void OnConsume_Slave(GamePlayerController byPlayer) {}
}
