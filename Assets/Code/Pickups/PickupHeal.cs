
public class PickupHeal : PickupCallbacks
{
    protected override void OnConsume_Master(PlayerPawn byPlayer)
    {
        byPlayer.Health.Revive_Master();
    }
}
