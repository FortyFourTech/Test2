using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PickupBase : MonoBehaviourPun
{
    // public Action onSpawned;
    public Action<PlayerPawn> onConsumed;

    private void OnTriggerEnter(Collider other)
    {
        Player hitPlayer = null;
        if (PhotonNetwork.IsMasterClient && HitPrecondition(other, ref hitPlayer))
        {
            ConsumePickup_Master(hitPlayer);
        }
    }
    
    private bool HitPrecondition(Collider other, ref Player hitPlayer)
    {
        var otherC = other.GetComponent<PlayerPawn>();
        if (otherC)
        {
            hitPlayer = otherC.Owner;

            return true;
        }

        return false;
    }

    private void ConsumePickup_Master(Player hitPlayer)
    {
        photonView.RPC("ConsumePickup_All", RpcTarget.AllViaServer, hitPlayer);
    }

    [PunRPC]
    private void ConsumePickup_All(Player byPlayer)
    {
        onConsumed?.Invoke(byPlayer.References().m_pawn);

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }
}
