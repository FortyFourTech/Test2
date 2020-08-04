using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.Collections;

public static class PlayerProperties
{
    public static class Keys
    {
        public const string MaxHealth = "max_health";
        public const string CurrentHealth = "cur_health";
        public const string Score = "score";
    }

    #region Public interface
    public static void ResetProperties(this Player player)
    {
        player.MaxHealth(0);
        player.CurHealth(0);
        player.Score(0);
    }

    public static float MaxHealth(this Player player)
    {
        return _GetProperty<float>(player, Keys.MaxHealth);
    }
    public static void MaxHealth(this Player player, float newVal)
    {
        _SetProperty(player, Keys.MaxHealth, newVal);
    }

    public static float CurHealth(this Player player)
    {
        return _GetProperty<float>(player, Keys.CurrentHealth);
    }
    public static void CurHealth(this Player player, float newVal)
    {
        _SetProperty(player, Keys.CurrentHealth, newVal);
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
    public GamePlayerController m_Controller;

    public PlayerReferences(GamePlayerController controller = null)
    {
        m_Controller = controller;
    }
}

public class PlayerReplicatedProperties : MonoBehaviourPunCallbacks
{
    #region Private fields
    private Player _playerWatched;

    // gameplay properties
    [ReadOnly] public float _maxHealth = 0f;
    [ReadOnly] public float _curHealth = 0f;
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
        _maxHealth = _playerWatched.MaxHealth();
        _curHealth = _playerWatched.CurHealth();
        _score = _playerWatched.Score();
    }
    #endregion
}
