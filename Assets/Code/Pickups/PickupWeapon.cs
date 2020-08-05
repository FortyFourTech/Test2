using UnityEngine;

public class PickupWeapon : PickupCallbacks
{
    [SerializeField] private int _weaponId;
    
    protected override void OnConsume_Master(PlayerPawn byPlayer)
    {
        // switch to weapon and restore ammo
        var weapon = byPlayer.Weapons.GetWeapon(_weaponId);
        weapon.Magazine.LoadFullMag_Master();
        byPlayer.Weapons.SetWeaponSelected_Master(_weaponId);
    }
}
