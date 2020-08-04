using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomParamsSetter : MonoBehaviour, IInRoomCallbacks
{
    [SerializeField]
    private RoomParams _defaultParams; // this is default
    private RoomParams _roomParams;

    [Header("sliders")]
    [SerializeField] private Slider _roundScoreSlider;
    [SerializeField] private Slider _hpSlider;

    private void Start()
    {
        if (!PhotonNetwork.InRoom)
        {
            gameObject.SetActive(false);
            return;    
        }
        
        _roomParams = RoomParams.LoadFromDevice(_defaultParams);

        _ApplyToUI();
        AcceptParameters();
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void AcceptParameters()
    {
        if (PhotonNetwork.InRoom)
            _roomParams.ApplyForPhoton(PhotonNetwork.CurrentRoom);
        
        _roomParams.SaveToDevice();
    }

    public void ResetParameters()
    {
        _roomParams = _defaultParams;
        _ApplyToUI();
    }

    public void SetMaxRoundScore(float maxScore)
    {
        _roomParams.maxRoundScore = Mathf.RoundToInt(maxScore);
    }

    public void SetPlayerHp(float hpVal)
    {
        _roomParams.playerHp = hpVal;
    }

    private float TextToFloat(Text text)
    {
        return float.Parse(text.text);
    }

    private void _ApplyToUI()
    {
        _hpSlider.value = _roomParams.playerHp;
        _roundScoreSlider.value = _roomParams.maxRoundScore;
    }


    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) 
    {
        _roomParams = RoomParams.GetFromPhoton();
        _ApplyToUI();
    }

    public void OnPlayerEnteredRoom(Player newPlayer) {}
    public void OnPlayerLeftRoom(Player otherPlayer) {}
    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps) {}
    public void OnMasterClientSwitched(Player newMasterClient) {}
}
