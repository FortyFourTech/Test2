using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;

public class GameMode : MonoBehaviourPunCallbacks
{
    [SerializeField] private int _maxScore = 10;
    [SerializeField] private PlayerSpawner _spawner;

    // подписаться на смерть игрока при спавне
    public override void OnEnable()
    {
        base.OnEnable();

        var spawnedPawns = FindObjectsOfType<PlayerPawn>();
        foreach (var pawn in spawnedPawns)
        {
            OnSpawn(pawn);
        }

        PlayerPawn.onSpawned += OnSpawn;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        PlayerPawn.onSpawned -= OnSpawn;
    }

    private void OnSpawn(PlayerPawn pawn)
    {
        pawn.Health.onKilled += OnPawnDeath;

        pawn.Owner.Score(0);

        OnPawnDeath(pawn.Health, null);
    }

    private async void OnPawnDeath(Health health, Player causer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            await Task.Delay(500);
            
            var pawn = health.GetComponent<PlayerPawn>();
            var deadPlayer = pawn.Owner;

            _spawner.RespawnPlayer_Master(deadPlayer);

            if (causer != null)
            {
                var newPlayerScore = causer.Score()+1;
                causer.Score(newPlayerScore);

                if (newPlayerScore >= _maxScore)
                {
                    Debug.Log("end of game");
                    if (PhotonNetwork.IsMasterClient)
                    {
                        photonView.RPC("LeaveRoom_All", RpcTarget.All);
                    }
                }
                else
                {
                    Debug.Log($"new player {causer.NickName} score is {newPlayerScore}");
                }
            }
        }
    }

    [PunRPC]
    public void LeaveRoom_All()
    {
        PhotonNetwork.LeaveRoom(); // load menu scene will be called from PlayerSpawner
    }
}
