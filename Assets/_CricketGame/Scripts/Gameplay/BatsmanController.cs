using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HitAnimation
{
    public string ParameterName;
    public AnimationClip Clip;
    public HitDirection HitDirection;
}

public enum HitDirection
{
    Left,
    Right,
}

public class BatsmanController : PlayerController
{
    [Header("Data")]
    [SerializeField] private HitAnimation[] hitAnimations;

    [Header("Dependencies")]
    [SerializeField, Required] private Animator animator;

    #region [Animation Hashes]

    public static readonly Dictionary<string, int> animationHashes = new Dictionary<string, int>();

    #endregion

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private bool hitInProgress;


 

    // Start is called before the first frame update
    void Start()
    {
        InitializeAnimationHashes();
        startingRotation = transform.rotation;
        startingPosition = transform.position;
    }

    private void InitializeAnimationHashes()
    {
        foreach (var hitAnimation in hitAnimations)
        {
            if (!animationHashes.ContainsKey(hitAnimation.ParameterName))
            {
                int hash = Animator.StringToHash(hitAnimation.ParameterName);
                animationHashes.Add(hitAnimation.ParameterName, hash);
            }
        }
    }

    private void AnimSetTrigger(string animationName)
    {
        if (animationHashes.TryGetValue(animationName, out int hash))
        {
            animator.SetTrigger(hash);
        }
        else
        {
            Debug.LogWarning($"Animation '{animationName}' not found in the hash dictionary.");
        }
    }

    public void OnHit(Component sender, object[] payload)
    {
        if (!PhotonView.IsMine) return;

        HitDirection hitDirection = (HitDirection)payload[0];
        if (!hitInProgress)
        {
            StartCoroutine(PlayHitAnimationAndWait(hitDirection));
        }
        else
        {
            Debug.LogWarning("Hit already in progress.");
        }
    }

    private IEnumerator PlayHitAnimationAndWait(HitDirection hitDirection)
    {
        hitInProgress = true;

        // Filter animations by the given hit direction
        List<HitAnimation> filteredAnimations = FilterAnimationsByDirection(hitDirection);

        // Check if any animations were found
        if (filteredAnimations.Count > 0)
        {
            // Pick a random animation from the filtered list
            HitAnimation selectedAnimation = SelectRandomAnimation(filteredAnimations);
            AnimSetTrigger(selectedAnimation.ParameterName);

            // Wait for the current animation to finish
            yield return StartCoroutine(WaitForAnimation(selectedAnimation.Clip));

            // Animation finished playing, reset hitInProgress and transform
            hitInProgress = false;
            ResetTransform();
        }
        else
        {
            Debug.LogWarning($"No animations found for hit direction '{hitDirection}'.");
            hitInProgress = false;
        }
    }

    private IEnumerator WaitForAnimation(AnimationClip clip)
    {
        animator.Play(clip.name); // Restart the animation in case it was interrupted
        yield return new WaitForSeconds(clip.length); // Wait for the animation to complete
    }

    private void ResetTransform()
    {
        transform.DOMove(startingPosition, 0.5f).SetEase(Ease.OutQuad);
        transform.DORotate(startingRotation.eulerAngles, 0.5f).SetEase(Ease.OutQuad);
    }

    private List<HitAnimation> FilterAnimationsByDirection(HitDirection hitDirection)
    {
        List<HitAnimation> filteredAnimations = new List<HitAnimation>();
        foreach (var hitAnimation in hitAnimations)
        {
            if (hitAnimation.HitDirection == hitDirection)
            {
                filteredAnimations.Add(hitAnimation);
            }
        }
        return filteredAnimations;
    }

    private HitAnimation SelectRandomAnimation(List<HitAnimation> animations)
    {
        return animations[Random.Range(0, animations.Count)];
    }

    private float GetAnimationLength(string animationName)
    {
     
       AnimationClip clip = animator.runtimeAnimatorController.animationClips[System.Array.FindIndex(animator.runtimeAnimatorController.animationClips, x => x.name.Equals(animationName))];
       return clip.length;
    }
}
