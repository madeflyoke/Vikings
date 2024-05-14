using System.Collections.Generic;
using Components;
using Components.Animation;
using Components.Interfaces;
using Components.Settings;
using Interfaces;
using UnityEngine;

namespace Factories.Decorators
{
    public class AnimationComponentDecorator : IEntityDecorator
    {
        private readonly AnimationComponentSettings _animationComponentSettings;
        private readonly EntityHolder _entityHolder;
        
        public AnimationComponentDecorator(EntityHolder entityHolder, AnimationComponentSettings animationComponentSettings)
        {
            _animationComponentSettings = animationComponentSettings;
            _entityHolder = entityHolder;
        }

        public IEntityComponent Decorate()
        {
            var animationComponent = CreateAnimationComponent();
            return animationComponent;
        }

        private AnimationComponent CreateAnimationComponent()
        {
            var entityHolderObj = _entityHolder.SelfTransform.gameObject;
            
            var animator = entityHolderObj.AddComponent<Animator>();
            animator.avatar = _animationComponentSettings.Avatar;
            animator.runtimeAnimatorController = CreateOverrideController();

            var animationEventsListener = entityHolderObj.AddComponent<AnimationEventsListener>();
            
            return new AnimationComponent(animator, animationEventsListener);
        }

        private AnimatorOverrideController CreateOverrideController()
        {
            AnimatorOverrideController controllerCopy =
                new AnimatorOverrideController(_animationComponentSettings.OverrideAnimatorController); 

            var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            _animationComponentSettings.OverrideAnimatorController.GetOverrides(overrides);
            controllerCopy.ApplyOverrides(overrides);
            return controllerCopy;
        }
    }
}
