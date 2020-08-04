using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FirearmSFX : FirearmCallbacks
{
    [SerializeField] protected AudioSource _source;
    [SerializeField] protected AudioClip[] shotAudio;
    [SerializeField] protected AudioClip[] emptyShotAudio;

    protected override void OnFireEmpty()
    {
        if (emptyShotAudio.Length > 0)
            _source.PlayOneShot(emptyShotAudio.GetRandomElement());
    }

    protected override void OnFire()
    {
        if (shotAudio.Length > 0)
            _source.PlayOneShot(shotAudio.GetRandomElement());
    }
}
