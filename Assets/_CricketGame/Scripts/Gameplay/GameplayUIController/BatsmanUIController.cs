using HyyderWorks.EventSystem;
using UnityEngine;
using UnityEngine.UI;

public class BatsmanUIController : MonoBehaviour
{
    [SerializeField] private Button leftHitBtn;
    [SerializeField] private Button rightHitBtn;


    [Header("Events")]
    [SerializeField] private HWEvent OnHit;


    private void Awake()
    {
        leftHitBtn.onClick.AddListener(OnLeftHit);
        rightHitBtn.onClick.AddListener(OnRightHit);
    }

    private void OnLeftHit()
    {
        OnHit.Raise(this, new object[] { HitDirection.Left });
    }

    private void OnRightHit()
    {
        OnHit.Raise(this, new object[] { HitDirection.Right});
    }

}
