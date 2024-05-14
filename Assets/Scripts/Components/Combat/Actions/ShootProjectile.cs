using BehaviorDesigner.Runtime.Tasks;
using Components.Combat.Actions.Setups;
using Components.Combat.Interfaces;

namespace Components.Combat.Actions
{
    public class ShootProjectile : CombatAction
    {
        private ShootProjectileSetup _shootProjectileSetup;

        public override void Initialize(CommonCombatActionSetup commonSetup) 
        {
            base.Initialize(commonSetup);
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
