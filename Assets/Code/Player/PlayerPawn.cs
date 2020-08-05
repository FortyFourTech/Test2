using System;
using Photon.Pun;
using Photon.Realtime;

public class PlayerPawn : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public static Action<PlayerPawn> onSpawned;

    private Player _owner;
    private Health _health;
    private WeaponCollection _weapons;

    public Health Health => _health;
    public WeaponCollection Weapons => _weapons;
    public Player Owner => photonView.Owner;

    private void Start()
    {
        onSpawned?.Invoke(this);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = new PlayerReferences(this);
        _owner = photonView.Owner;
        _health = GetComponent<Health>();
        _weapons = GetComponent<WeaponCollection>();

    }
}
