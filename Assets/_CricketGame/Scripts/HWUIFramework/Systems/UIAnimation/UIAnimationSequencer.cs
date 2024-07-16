using System.Collections;
using UnityEngine;

namespace HyyderWorks.UI.Animation
{
    [System.Serializable]
    public struct AnimationSequenceData
    {
        public string SequenceName;
        public float Length;
        public UIAnimationPlayer AnimationPlayer;
    }

    /// <summary>
    /// Class to sequence different UI Animation Players
    /// </summary>
    public class UIAnimationSequencer : MonoBehaviour
    {
        [SerializeField] private AnimationSequenceData[] sequenceData;
        [SerializeField] private float initialDelay;
        private IEnumerator DoPlaySequence(bool reverse)
        {
            yield return new WaitForSeconds(initialDelay);

            int startIndex = reverse ? sequenceData.Length - 1 : 0;
            int endIndex = reverse ? -1 : sequenceData.Length;

            for (int i = startIndex; reverse ? i >= endIndex : i < endIndex; i += reverse ? -1 : 1)
            {
                AnimationSequenceData data = sequenceData[i];
                HWLogger.Log(GetType(), $"Playing Animations for {data.SequenceName}");
                data.AnimationPlayer.PlayWithDelay(reverse);
                yield return new WaitForSeconds(data.Length);
            }
        }

        public void PlaySequence()
        {
            StartCoroutine(DoPlaySequence(false));
        }

        public void PlaySequenceReverse()
        {
            StartCoroutine(DoPlaySequence(true));
        }
    }
}
