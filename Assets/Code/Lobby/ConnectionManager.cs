using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private string loadSceneName;

    private static ConnectionManager _instance = null;
    public static ConnectionManager Instance => _instance;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);

        // Critical
        // This makes sure we can use PhotonNetwork.LoadLevel() on the master client 
        // and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        // Critical
        // Connect to the Photon Network (server) 
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.NickName = NetworkPlayerUtils.NewNickName;        // Assign a nickname to ID player in room

        TryConnectCloud();
    }

    public void CreateRoom(string sceneName, int maxPlayersPerRoom = 10)
    {
        loadSceneName = sceneName;
        PhotonNetwork.CreateRoom(null, new RoomOptions() {
            MaxPlayers = (byte)maxPlayersPerRoom,
            // BroadcastPropsChangeToAll = true,
        });
    }

    public void TryConnectCloud()
    {
        Debug.Log("connecting cloud");
        
        PhotonNetwork.NickName = NetworkPlayerUtils.NewNickName;        // Assign a nickname to ID player in room
        PhotonNetwork.NetworkingClient.UserId = PhotonNetwork.NickName;

        PhotonNetwork.ConnectUsingSettings();
    }

#region PUN callbacks
    public override void OnJoinedRoom()
    {
        // Critical
        // We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` 
        // to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Loading Room");

            // Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel(loadSceneName);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("disconnected because: " + cause.ToString());
        // try to reconnect
        TryConnectCloud();
    }
#endregion
}
