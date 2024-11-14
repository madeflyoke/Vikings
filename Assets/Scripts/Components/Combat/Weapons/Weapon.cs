using System;
using System.Collections.Generic;
using System.Linq;
using Components.Combat.Actions.Conditions;
using Components.Combat.Weapons.Enums;
using Components.Combat.Weapons.Interfaces;
using Components.View;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

namespace Components.Combat.Weapons
{
    public class Weapon : SerializedMonoBehaviour
    {
        public WeaponData WeaponData { get; private set; }
        public bool Activated { private set; get; }
        
        [SerializeField] private List<IWeaponAttackHandler> _weaponActionHandlers;
        private HumanoidModelHolder _humanoidModelHolder;

        public void Initialize()
        {
            _weaponActionHandlers.ForEach(x=>x.Initialize(this));
        }
        
        public void ActivateWeapon(bool value)
        {
            Activated = value;
            gameObject.SetActive(value);
        }
        
        public void SetData(WeaponData weaponData, HumanoidModelHolder humanoidModelHolder)
        {
            WeaponData = weaponData;
            _humanoidModelHolder = humanoidModelHolder;
        }

        public void SwitchHandParent()
        {
            transform.parent = transform.parent == _humanoidModelHolder.LeftHandPoint
                ? _humanoidModelHolder.RightHandPoint
                : _humanoidModelHolder.LeftHandPoint;
            transform.localPosition = Vector3.zero;
        }
        
        public void SetTarget(DamageableTarget target)
        {
            _weaponActionHandlers.ForEach(x=>x.SetTarget(target));
        }

        public T GetAttackHandler<T>() where T: IWeaponAttackHandler
        {
            return (T)_weaponActionHandlers.FirstOrDefault(x => x.GetType() == typeof(T));
        }

        public bool ValidateWeaponByConditions(CombatActionConditions conditions)
        {
            if (conditions.WeaponType!=WeaponData.Type)
            {
                return false;
            }
            
            foreach (var conditionHandler in conditions.AttackHandlers)
            {
                var resultHandler = _weaponActionHandlers.FirstOrDefault(x => x.GetType() == conditionHandler);
                if (resultHandler==null)
                {
                    Debug.LogError($"Missing attack handler by condition: {conditionHandler}");
                    return false;
                }
            }

            return true;
        }
    }
}
