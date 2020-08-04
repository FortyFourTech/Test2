using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// Располагать на руте игрока, вместе с PhotonView
/// </summary>
public class WeaponCollection : MonoBehaviourPun
{
    public System.Action onSelected;

    [SerializeField] private Firearm[] _weapons;

    // Для дебага сделана возможность начать с выбранным оружием
    [SerializeField] bool _useDefault;
    [SerializeField] int _defaultIdx;

    private Firearm _currentWeapon = null;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var selectedIdx = _useDefault
                ? _defaultIdx
                : Random.Range(0, _weapons.Length);
            var selectedId = _weapons[selectedIdx].ID;

            SetWeaponSelected_Master(selectedId);
        }
    }

    public Firearm GetWeapon(int wId)
    {
        var result = _weapons.Where(w => w.ID == wId).FirstOrDefault();

        return result;
    }
    
    public void SetWeaponSelected_Master(int wId)
    {
        photonView.RPC("SetWeaponSelected_All", RpcTarget.AllBuffered, wId);
    }

    [PunRPC]
    public void SetWeaponSelected_All(int wId)
    {
        var prevWeapon = _currentWeapon;

        // current disable
        if (_currentWeapon)
        {
            _currentWeapon.SetSelected(false);
        }

        // selected enable
        _currentWeapon = GetWeapon(wId);
        if (_currentWeapon)
        {
            _currentWeapon.SetSelected(true);
        }
        else
        {
            Debug.LogError("selected weapon id is absent: " + wId);
        }

        if (_currentWeapon != prevWeapon)
        {
            onSelected?.Invoke();
        }
    }

}
