using UnityEngine;

public class PickupWeapon : PickupCallbacks
{
    [SerializeField] private int _weaponId;
    
    protected override void OnConsume_Master(GamePlayerController byPlayer)
    {
        // switch to weapon and restore ammo
        // byPlayer.Weapons.GetWeapon(_weaponId).Magazine.LoadFullMag_Master();
        byPlayer.Weapons.SetWeaponSelected_Master(_weaponId);
    }
}
