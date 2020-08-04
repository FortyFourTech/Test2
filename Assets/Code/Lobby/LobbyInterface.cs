using Photon.Pun;
using UnityEngine;

public class LobbyInterface : MonoBehaviourPunCallbacks
{
    [SerializeField] private int _maxPlayersPerRoom = 2;      // Set to 2 by default
    [SerializeField] private LobbyControl _controller;

    
    [Header("Selection Panel")]
    [SerializeField] private GameObject _selectionPanel;

    [Header("Room List Panel")]
    [SerializeField] private RoomList roomListPanel;
    
    #region PUN CALLBACKS
    public override void OnLeftRoom()
    {
        SetActivePanel(_selectionPanel.name);
    }
    #endregion

    #region UI CALLBACKS

    public void OnCreateRoomButtonClicked()
    {
        _controller.CreateRoom(_maxPlayersPerRoom);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        _controller.JoinRandomRoom();
    }

    public void OnStartGameButtonClicked()
    {
        _controller.StartGame();
    }
    #endregion


    private void SetActivePanel(string activePanel)
    {
        _selectionPanel.SetActive(activePanel.Equals(_selectionPanel.name));
        roomListPanel?.SetActive(activePanel.Equals(roomListPanel.gameObject.name));    // UI should call OnRoomListButtonClicked() to activate this
    }
}
