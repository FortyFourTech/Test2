using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.Collections;

public static class PlayerProperties
{
    public static class Keys
    {
        public const string Score = "score";
    }

    #region Public interface
    public static void ResetProperties(this Player player)
    {
        player.Score(0);
    }

    public static int Score(this Player player)
    {
        return _GetProperty<int>(player, Keys.Score);
    }
    public static void Score(this Player player, int newVal)
    {
        _SetProperty(player, Keys.Score, newVal);
    }
    
    // probably unnecessary // delete in the future
    public static PlayerReferences References(this Player player)
    {
        if (player.TagObject != null && player.TagObject is PlayerReferences)
            return (PlayerReferences)player.TagObject;
        else
            return default(PlayerReferences);
    }
    #endregion

    #region Private functions
    private static T _GetProperty<T>(Player player, string propName, T defaultVal = default(T))
    {
        object propValueObj;
        if (player.CustomProperties.TryGetValue(propName, out propValueObj))
        {
            T propValue = (T)propValueObj;
            return propValue;
        }
        else
        {
            return defaultVal;
        }
    }

    private static void _SetProperty<T>(Player player, string propName, T propValue, bool useCas = true)
    {
        // Debug.Log($"setting {propName} to {propValue}");
        Hashtable newProps = new Hashtable {
            {propName, propValue}
        };

        Hashtable oldProps = null;
        
        if (useCas)
        {
            object prevVal;
            if (player.CustomProperties.TryGetValue(propName, out prevVal))
            {
                // Debug.Log($"prev value of {propName} is {prevVal}");
                oldProps = new Hashtable() {
                    {propName, prevVal}
                };
            }
        }

        player.SetCustomProperties(newProps, oldProps);
    }
    #endregion
}

public struct PlayerReferences
{
    public PlayerPawn m_pawn;

    public PlayerReferences(PlayerPawn pawn = null)
    {
        m_pawn = pawn;
    }
}

public class PlayerReplicatedProperties : MonoBehaviourPunCallbacks
{
    #region Private fields
    private Player _playerWatched;

    // gameplay properties
    [ReadOnly] public int _score = 0;
    #endregion

    #region Unity / Photon functions
    private void Start()
    {
        _playerWatched = photonView.Owner;
        _UpdateProperties();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == _playerWatched)
        {
            _UpdateProperties();
        }
    }
    #endregion

    #region Private functions
    private void _UpdateProperties()
    {
        _score = _playerWatched.Score();
    }
    #endregion
}
