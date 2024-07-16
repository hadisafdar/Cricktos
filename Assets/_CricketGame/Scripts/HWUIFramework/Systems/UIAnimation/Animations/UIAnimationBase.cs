namespace HyyderWorks.UI.Animation
{
    using DG.Tweening;
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    public abstract class UIAnimationBase : MonoBehaviour
    {

        [SerializeField] protected float duration = 1f;
        [SerializeField] protected Ease easeType = Ease.OutExpo;

        [SerializeField] protected UnityEvent OnAnimationStart;
        [SerializeField] protected UnityEvent OnAnimationEnd;


        public float Duration => duration;

        public abstract void Play(Action onComplete);
        public abstract void PlayReverse(Action onComplete);
    }

}