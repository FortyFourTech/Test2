using UnityEngine;

public class PickupHeal : PickupCallbacks
{
    protected override void OnConsume_Master(GamePlayerController byPlayer)
    {
        byPlayer.Health.Revive_Master();
    }
}
