using System;
using Photon.Pun;
using UnityEngine;

public class Firearm : MonoBehaviourPun
{
    public Action onFire;
    public Action onFireEmpty;
    
    [SerializeField] private int _id;

    [SerializeField] private Magazine _magazine;

    public int ID => _id;
    public Magazine Magazine => _magazine;

    private void Update()
    {
        if (!photonView.IsMine) return;

        bool input = Input.GetMouseButtonDown(0);

        if (input)
        {
            PullTrigger_Owner();
        }
    }

#region Trigger
    private void PullTrigger_Owner()
    {
        bool canFire = FirePreconditions();
        if (!canFire)
        {
            MakeEmptyShot_Owner();
        }
        else
        {
            MakeShot_Owner();
        }
    }
#endregion // Trigger

    private bool FirePreconditions()
    {
        if (_magazine.Empty)
            return false;
        
        return true;
    }

#region Firing
    public bool MakeShot_Owner()
    {
        if (!FirePreconditions()) return false;

        _magazine.EjectBullet_Owner();

        // Debug.Log("last shot time: " + Time.time.ToString(), this);

        photonView.RPC("MakeShot_All", RpcTarget.All);
        PhotonNetwork.SendAllOutgoingCommands();

        return true;
    }

    [PunRPC]
    protected virtual void MakeShot_All()
    {
        onFire?.Invoke();
    }

    protected void MakeEmptyShot_Owner()
    {
        photonView.RPC("MakeEmptyShot_All", RpcTarget.All);
    }

    [PunRPC]
    protected void MakeEmptyShot_All()
    {
        onFireEmpty?.Invoke();
    }
#endregion // Firing

    public void SetSelected(bool state)
    {
        gameObject.SetActive(state);
    }
}
