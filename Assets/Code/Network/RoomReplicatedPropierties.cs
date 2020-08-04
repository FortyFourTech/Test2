using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

static class RoomProperties
{
    public static class Keys
    {
        // config
        public const string MaxRoundScore = "RoundScoreMax";
        public const string MaxPlayerHealth = "MaxPlayerHealth";
    }

    private static T _GetProperty<T>(Room room, string propName, T defaultVal)
    {
        if (room.CustomProperties.ContainsKey(propName))
        {
            return (T)room.CustomProperties[propName];
        }
        else
        {
            return defaultVal;
        }
    }

    private static void _SetProperty<T>(Room room, string propName, T val)
    {
        var props = new Hashtable();
        props[propName] = val;
        room.SetCustomProperties(props);
    }

    public static void ResetProperties(this Room room)
    {
        PhotonNetwork.RemoveRPCsInGroup(0);

        Hashtable clearRoomProps = new Hashtable();

        room.SetCustomProperties(clearRoomProps);
    }

    public static int GetMaxRoundScore(this Room room)
    {
        return _GetProperty(room, Keys.MaxRoundScore, 0);
    }
    public static void SetMaxRoundScore(this Room room, int newVal)
    {
        _SetProperty(room, Keys.MaxRoundScore, newVal);
    }

    public static float GetPlayerHealth(this Room room)
    {
        return _GetProperty(room, Keys.MaxPlayerHealth, 100f);
    }
    public static void SetPlayerHealth(this Room room, float newVal)
    {
        _SetProperty(room, Keys.MaxPlayerHealth, newVal);
    }
}
