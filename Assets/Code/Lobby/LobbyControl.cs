using Photon.Pun;
using UnityEngine;

public class LobbyControl : MonoBehaviourPunCallbacks
{
#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 30), "create"))
        {
            CreateRoom(10);
        }
        if (GUI.Button(new Rect(0, 30, 100, 30), "join"))
        {
            JoinRandomRoom();
        }
        if (GUI.Button(new Rect(0, 60, 100, 30), "start"))
        {
            StartGame();
        }
    }
#endif

    #region UI CALLBACKS

    public void CreateRoom(int maxPlayers)
    {
        if (!PhotonNetwork.IsConnected)
        {
            ConnectionManager.Instance.TryConnectCloud();
        }
        else
        {
            ConnectionManager.Instance.CreateRoom(RoomName.LOBBY_ROOM, maxPlayers);
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void StartGame()
    {
        // PhotonNetwork.CurrentRoom.IsOpen = false;
        // PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel(RoomName.GAME_ROOM);
    }
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateRoom(2);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            JoinRandomRoom();
        }
    }
}
