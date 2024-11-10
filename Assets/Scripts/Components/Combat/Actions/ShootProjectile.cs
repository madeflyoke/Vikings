using BehaviorDesigner.Runtime.Tasks;
using BT.Shared;
using Components.Combat.Actions.Setups;
using Components.Combat.Interfaces;
using Components.Combat.Weapons;

namespace Components.Combat.Actions
{
    public class ShootProjectile : CombatAction
    {
        private ShootProjectileSetup _shootProjectileSetup;

        public override void Initialize(CommonCombatActionSetup commonSetup, Weapon weapon) 
        {
            base.Initialize(commonSetup, weapon);
            _shootProjectileSetup = commonSetup as ShootProjectileSetup;
        }

        public override TaskStatus GetCurrentStatus()
        {
            return TaskStatus.Running;
        }

        public override void Execute()
        {
            
        }

       
    }
}
