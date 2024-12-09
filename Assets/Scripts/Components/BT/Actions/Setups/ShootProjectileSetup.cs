using Components.Animation;
using Components.BT.Actions.Attributes;

namespace Components.BT.Actions.Setups
{
    [BehaviorActionVariant(ActionVariant = typeof(ShootProjectileAttack))]
    public class ShootProjectileSetup : CommonWeaponActionSetup
    {
        public AnimationClipData ReloadAnimation;
    }
}