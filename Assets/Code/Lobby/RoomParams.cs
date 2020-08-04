using UnityEngine;
using Photon.Realtime;

[System.Serializable]
public struct RoomParams
{
    public int maxRoundScore;
    public float playerHp;

    public static RoomParams GetFromPhoton()
    {
        RoomParams playerParams = new RoomParams();

        var room = Photon.Pun.PhotonNetwork.CurrentRoom;
        if (room == null)
        {
            return playerParams;
        }

        playerParams.maxRoundScore = room.GetMaxRoundScore();
        playerParams.playerHp = room.GetPlayerHealth();

        return playerParams;
    }

    public void ApplyForPhoton(Room toRoom)
    {
        toRoom.SetMaxRoundScore(this.maxRoundScore);
        toRoom.SetPlayerHealth(this.playerHp);
    }

    public static RoomParams LoadFromDevice(RoomParams defaultValues)
    {
        RoomParams playerParams = new RoomParams();

        playerParams.maxRoundScore = PlayerPrefs.GetInt(RoomProperties.Keys.MaxRoundScore, defaultValues.maxRoundScore);
        playerParams.playerHp = PlayerPrefs.GetFloat(RoomProperties.Keys.PlayerMaxHealth, defaultValues.playerHp);

        return playerParams;
    }

    public void SaveToDevice()
    {
        PlayerPrefs.SetInt(RoomProperties.Keys.MaxRoundScore, this.maxRoundScore);
        PlayerPrefs.SetFloat(RoomProperties.Keys.PlayerMaxHealth, this.playerHp);

        PlayerPrefs.Save();
    }
}
