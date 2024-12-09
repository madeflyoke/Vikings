using System;
using System.Collections.Generic;
using System.Linq;
using Components.BT.Actions.Conditions;
using Components.Combat.Interfaces;
using Components.Combat.Weapons.Enums;
using Components.Combat.Weapons.Interfaces;
using Components.View;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Utility;

namespace Components.Combat.Weapons
{
    public class Weapon : SerializedMonoBehaviour, IWeaponStatsProvider
    {
        public WeaponType WeaponType => _weaponData.Type;
        
        [SerializeField] private List<IWeaponAttackHandler> _weaponActionHandlers;
        private HumanoidModelHolder _humanoidModelHolder;
        private WeaponData _weaponData;
        private bool _activated;

        public void Initialize()
        {
            _weaponActionHandlers.ForEach(x=>x.Initialize());
        }
        
        public void SetWeaponActive(bool value)
        {
            _activated = value;
            gameObject.SetActive(value);
            _weaponActionHandlers.ForEach(x=>x.OnWeaponStateChanged(value));
        }
        
        public void SetData(WeaponData weaponData, HumanoidModelHolder humanoidModelHolder)
        {
            _weaponData = weaponData;
            _humanoidModelHolder = humanoidModelHolder;
        }

        public void SetHandParent(bool mainHand)
        {
            transform.parent = mainHand ? _humanoidModelHolder.RightHandPoint : _humanoidModelHolder.LeftHandPoint;
            transform.localPosition = Vector3.zero;
            
            var rot = transform.localRotation.eulerAngles;
            rot.y = mainHand ? 0 : 180f;
            transform.localRotation = Quaternion.Euler(rot);
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
            if (conditions.WeaponType!=_weaponData.Type)
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

        public WeaponStats GetCombatStats()
        {
            return _weaponData.Stats;
        }
        
        #if UNITY_EDITOR

        private void OnValidate()
        {
            if (_weaponActionHandlers.IsNullOrEmpty()==false)
            {
                _weaponActionHandlers.ForEach(x => x.EDITOR_ManualValidate());
                EditorUtility.SetDirty(gameObject);
            }
        }

#endif
    }
}
