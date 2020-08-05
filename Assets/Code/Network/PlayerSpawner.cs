using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing the player")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private PlayerSpawnPoint[] _points;

#region Photon Callbacks
    /// <summary>
    /// Photon callback advises that local player has left room so reload launcher scene
    /// </summary>
    public override void OnLeftRoom()
    {
        // Load 'first' scene (Launcher.unity)
        SceneManager.LoadScene(0);
    }
#endregion

#region Public and Private Methods
    private void Start()
    {
        _SpawnPlayer_Owner();
    }

    private void _SpawnPlayer_Owner()
    {
        if (_playerPrefab == null)
        {
            Debug.LogError("Missing playerPrefab reference...please set it up in GameObject 'Room Manager", this);
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.TagObject == null || PhotonNetwork.LocalPlayer.References().m_pawn == null)
            {
                Debug.LogFormat("Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                // Spawn a character for the local player
                // This gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
                PhotonNetwork.SendAllOutgoingCommands();
            }
            else
            {
                Debug.LogFormat("ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    /// <summary>
    /// Make local player leave the room on Photon server
    /// </summary>
    public static void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public void RespawnPlayer_Master(Player player)
    {
        var pointChosen = _points.GetRandomElement();
        pointChosen.Respawn_Master(player);
    }
#endregion

// #if UNITY_EDITOR
//     private void OnGUI()
//     {
//         if (GUI.Button(new Rect(0, 0, 100, 30), "leave"))
//         {
//             LeaveRoom();
//         }
//     }
// #endif
}
