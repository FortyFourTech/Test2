using Photon.Pun;
using Photon.Realtime;

public class GamePlayerController : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    private Player _owner;
    private Health _health;
    private WeaponCollection _weapons;

    public Health Health => _health;
    public WeaponCollection Weapons => _weapons;
    public Player Owner => photonView.Owner;

    private void Start()
    {
        _health = GetComponent<Health>();
        _weapons = GetComponent<WeaponCollection>();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = new PlayerReferences(this);
        _owner = photonView.Owner;
    }
}
