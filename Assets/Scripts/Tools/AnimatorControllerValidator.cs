using System.Collections.Generic;
using System.Linq;
using Components.Settings;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Utility;

namespace Tools
{
    public class AnimatorControllerValidator : StateMachineBehaviour
    {
#if UNITY_EDITOR
        
        [SerializeField] private AnimatorController _animatorController;
        [SerializeField] private string _path;
        
        [Button, Tooltip("Caution! Removes all files except empty animations in folder")]
        public void RecreateAndFillEmptyAnimations()
        {
            if (ValidateStatesNames()==false)
            {
                return;
            }
            
            var usedClips = new List<AnimationClip>();

            for (int z = 0; z < _animatorController.layers.Length; z++)
            {
                _animatorController.layers[z].stateMachine.states
                    .ForEach(x =>
                    {
                        var motion = _animatorController.GetStateEffectiveMotion(x.state);
                        if (motion != null && motion.name.Equals(x.state.name))
                        {
                            usedClips.Add(motion as AnimationClip);
                            return;
                        }

                        var tryingClip = TryGetEmptyClip(x.state.name);
                        if (tryingClip)
                        {
                            _animatorController.SetStateEffectiveMotion(x.state, tryingClip);
                            usedClips.Add(tryingClip);
                            return;
                        }
                        
                        var clip = new AnimationClip();
                        clip.name = x.state.name;
                        AssetDatabase.CreateAsset(clip, $"{_path}\\"+clip.name+".anim");
                        AssetDatabase.Refresh();
                        usedClips.Add(clip);
                        _animatorController.SetStateEffectiveMotion(x.state, clip);
                    });
                
            }

            DeleteUnnecessaryAnimations(usedClips);

            AssetDatabase.Refresh();
        }

        private AnimationClip TryGetEmptyClip(string targetName)
        {
            string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { _path });
            
            List<AnimationClip> assets = new();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                assets.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath));
            }

            return assets.FirstOrDefault(x => x.name.Equals(targetName));
        }
        
        private void DeleteUnnecessaryAnimations(List<AnimationClip> usedClips)
        {
            string[] guids = AssetDatabase.FindAssets("t:AnimationClip", new[] { _path });
            List<AnimationClip> assets = new();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                assets.Add(AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath));
            }
            
            assets.RemoveAll(usedClips.Contains);

            for (int i = assets.Count-1; i >= 0; i--)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(assets[i]));
            }
            AssetDatabase.Refresh();
        }

        private bool ValidateStatesNames()
        {
            var values = AnimatorStatesNames.GetAnimatorNamesValues();
            bool isOk = true;

            for (int z = 0; z < _animatorController.layers.Length; z++)
            {
                var names = _animatorController.layers[z].stateMachine.states.Select(x => x.state.name).ToArray();
            
                for (int i = 0; i < names.Length; i++)
                {
                    if (values.Contains(names[i])==false)
                    {
                        Debug.LogError($"Incorrect animator controller {_animatorController.name} state name <color=red>{names[i]}</color>, " +
                                       $"use <color=yellow>AnimatorStatesNames</color> class to choose one!");
                        isOk = false;
                    }
                }
                
            }
            return isOk;
        }
        
        private void OnValidate()
        {
            ValidateStatesNames();
        }

#endif
    }
}
