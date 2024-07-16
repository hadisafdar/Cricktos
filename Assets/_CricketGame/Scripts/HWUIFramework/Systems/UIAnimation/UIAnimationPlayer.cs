namespace HyyderWorks.UI.Animation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Manages the playback of a list of UIAnimationBase instances.
    /// </summary>
    public class UIAnimationPlayer : MonoBehaviour
    {
        [SerializeField] private List<UIAnimationBase> animations = new List<UIAnimationBase>(); // List of animations to play
        [SerializeField] private float delay;
        [SerializeField] private bool playOnStart;

        public event Action OnSequenceCompleted;

        private void Start()
        {
            if (playOnStart)
            {
                PlayWithDelay(false);
            }
        }



        /// <summary>
        /// Plays animation with delay.
        /// </summary>
        /// <param name="reverse">Whether to play in reverse direction.</param>
        public void PlayWithDelay(bool reverse)
        {
            StartCoroutine(DoPlayWithDelay(delay, reverse));
        }

        private IEnumerator DoPlayWithDelay(float delay, bool reverse)
        {
            int startIndex = reverse ? animations.Count - 1 : 0;
            int endIndex = reverse ? -1 : animations.Count;

            for (int i = startIndex; reverse ? i >= endIndex : i < endIndex; i += reverse ? -1 : 1)
            {
                UIAnimationBase animation = animations[i];
                HWLogger.Log(GetType(), "Playing With Delay: " + animation.GetType());
                animation.Play(null);
                yield return new WaitForSeconds(delay);
            }
            OnSequenceCompleted?.Invoke();
        }

    }
}
