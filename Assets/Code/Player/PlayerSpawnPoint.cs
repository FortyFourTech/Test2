using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawnPoint : MonoBehaviourPun
{
    public void Respawn_Master(Player player)
    {
        photonView.RPC("RespawnOnPoint_All", RpcTarget.All, player);
    }

    [PunRPC]
    public void RespawnOnPoint_All(Player player)
    {
        var pawn = player.References().m_pawn;
        if (pawn)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                var movementC = pawn.GetComponent<PlayerMovement>();
                movementC.Teleport(transform.position, transform.rotation);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                pawn.Health.Revive_Master();
            }
        }
    }
}
