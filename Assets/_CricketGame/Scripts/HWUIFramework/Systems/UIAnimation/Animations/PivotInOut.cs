namespace HyyderWorks.UI.Animation
{
    using DG.Tweening;
    using Sirenix.OdinInspector;
    using System;
    using UnityEngine;

    public class PivotInOut : UIAnimationBase
    {
        [SerializeField] private Vector2 startPivot; // Starting position for the animation
        [SerializeField] private Vector2 endPivot; // Ending position for the animation
        [SerializeField] private RectTransform target;

        private void Start()
        {
            target.pivot = startPivot;
        }

        /// <summary>
        /// Plays the slide-in animation.
        /// </summary>
        /// <param name="target">The RectTransform to animate.</param>
        /// <param name="onComplete">Action to execute upon completion of the animation.</param>
        public override void Play(Action onComplete)
        {
            OnAnimationStart?.Invoke(); // Invoke the start animation event if assigned
            target.pivot = startPivot;
            target.DOPivot(endPivot, duration)
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

            target.pivot = endPivot;

            target.DOPivot(startPivot, duration)
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