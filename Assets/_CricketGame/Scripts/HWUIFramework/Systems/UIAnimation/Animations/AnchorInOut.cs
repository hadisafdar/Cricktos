namespace HyyderWorks.UI.Animation
{
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    public class AnchorInOut : UIAnimationBase
    {
        [SerializeField] private Vector2 startAnchor; // Starting position for the animation
        [SerializeField] private Vector2 endAnchor; // Ending position for the animation
        [SerializeField] private RectTransform target;

        /// <summary>
        /// Plays the slide-in animation.
        /// </summary>
        /// <param name="target">The RectTransform to animate.</param>
        /// <param name="onComplete">Action to execute upon completion of the animation.</param>
        public override void Play(Action onComplete)
        {
            OnAnimationStart?.Invoke(); // Invoke the start animation event if assigned
            target.anchoredPosition = startAnchor;
            target.DOAnchorPos(endAnchor, duration)
                .SetEase(easeType)
                .OnComplete(() => {
                    OnAnimationEnd?.Invoke(); // Invoke the end animation event if assigned
                onComplete?.Invoke(); // Execute the onComplete action
            });
        }

        /// <summary>
        /// Reverses the slide-in animation, moving the RectTransform back to the start position.
        /// </summary>
        /// <param name="target">The RectTransform to animate.</param>
        /// <param name="onComplete">Action to execute upon completion of the reverse animation.</param>
        public override void PlayReverse(Action onComplete)
        {
            OnAnimationStart?.Invoke(); // Invoke the start animation event if assigned

            target.anchoredPosition = endAnchor;

            target.DOAnchorPos(startAnchor, duration)
                .SetEase(easeType)
                .OnComplete(() => {
                    OnAnimationEnd?.Invoke(); // Invoke the end animation event if assigned
                onComplete?.Invoke(); // Execute the onComplete action
            });
        }


        [Button("Fetch Target")]
        private void FetchTarget()
        {
            target = GetComponent<RectTransform>();
        }

    }

}