using Photon.Pun;
using Photon.Realtime;

public class GamePlayerController : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    private Player _owner;

    public Player GetOwner()
    {
        return photonView.Owner;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = new PlayerReferences(this);
        _owner = photonView.Owner;
    }
}
