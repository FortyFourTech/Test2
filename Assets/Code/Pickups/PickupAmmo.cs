using UnityEngine;

public class PickupAmmo : PickupCallbacks
{
    [SerializeField] private int _weaponId;

    protected override void OnConsume_Master(PlayerPawn byPlayer)
    {
        // restore ammo
        var weapon = byPlayer.Weapons.GetWeapon(_weaponId);
        weapon.Magazine.LoadFullMag_Master();
    }
}
