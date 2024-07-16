namespace HyyderWorks.UI.Animation
{
    using DG.Tweening;
    using System;
    using UnityEngine;


    public class FadeInOut : UIAnimationBase
    {
        [SerializeField] private bool enableInteractionsOnFadeIn = true; //should it make the canvas group interactable and block raycast when we fade in
        [SerializeField] private CanvasGroup canvasGroup; // Starting position for the animation
        /// <summary>
        /// Plays the slide-in animation.
        /// </summary>
        /// <param name="target">The RectTransform to animate.</param>
        /// <param name="onComplete">Action to execute upon completion of the animation.</param>

        private void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }

        public override void Play(Action onComplete)
        {

            OnAnimationStart?.Invoke(); // Invoke the start animation event if assigned
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = enableInteractionsOnFadeIn;
                    canvasGroup.blocksRaycasts = enableInteractionsOnFadeIn;
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
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    OnAnimationEnd?.Invoke(); // Invoke the end animation event if assigned
                onComplete?.Invoke(); // Execute the onComplete action
            });
        }
    }

}