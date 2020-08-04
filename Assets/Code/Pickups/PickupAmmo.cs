using UnityEngine;

public class PickupAmmo : PickupCallbacks
{
    [SerializeField] private int _weaponId;

    protected override void OnConsume_Master(GamePlayerController byPlayer)
    {
        // restore ammo
        byPlayer.Weapons.GetWeapon(_weaponId).Magazine.LoadFullMag_Master();
    }
}
