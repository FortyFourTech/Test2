using Photon.Pun;
using UnityEngine;

public class MagazineSFX : MagazineCallbacks
{
    [SerializeField] protected PhotonView _owner;
    [SerializeField] protected AudioSource _source;
    [SerializeField] protected AudioClip _emptySound;
    [SerializeField] protected AudioClip _reloadSound;

    protected override void OnEmpty()
    {
        _source.PlayOneShot(_emptySound);
    }

    protected override void OnReload()
    {
        _source.PlayOneShot(_reloadSound);
    }
}
