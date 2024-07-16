using HyyderWorks.EventSystem;
using System.Collections;
using UnityEngine;
using Photon.Pun;

public class BowlerController : PlayerController
{
    [Header("Data")]
    [SerializeField] private Vector2 bowlingXRange = new Vector2(-1, 1);
    [SerializeField] private BallController ballTemplate;

    [Header("Dependencies")]
    [SerializeField] private GameObject ballDisplay;
    [SerializeField] private Animator animator;

    [Header("Events")]
    [SerializeField] private HWEvent OnBowlingXChange;

    private float bowlingX;
    private float time;

    private bool isSelecting = true;
    private bool isBallThrown = false;

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    private static readonly int ANIM_FASTBALL = Animator.StringToHash("FastBall");


    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
       
        ballDisplay.SetActive(true);
    }

    private void Update()
    {
        if (!isSelecting) return;

        // Update the time to create a continuous oscillation
        time += Time.deltaTime;

        // Ping-pong the value of time to oscillate between 0 and 1
        float t = Mathf.PingPong(time, 1);

        // Interpolate between bowlingXRange.x and bowlingXRange.y using the ping-ponged time
        bowlingX = Mathf.Lerp(bowlingXRange.x, bowlingXRange.y, t);

        // Raise the event if OnBowlingXChange is assigned
        OnBowlingXChange?.Raise(this, new object[] { bowlingX });
    }

    public void OnSelect(Component sender, object[] payload)
    {
        if (!isSelecting || isBallThrown) return;
        if (!PhotonView.IsMine && PhotonPlayer.PlayerRole != PlayerRole.Bowler) return;

        isSelecting = false;
        StartBall();
    }

    public void StartBall()
    {
        StartCoroutine(C_StartBall());
    }

    private IEnumerator C_StartBall()
    {
        // Trigger the fast ball animation
        animator.SetTrigger(ANIM_FASTBALL);

        // Wait for the animation event to trigger the ball throw logic
        while (!isBallThrown)
        {
            yield return null;
        }
    }

    private void ThrowBall()
    {
        if (!PhotonView.IsMine && PhotonPlayer.PlayerRole != PlayerRole.Bowler) return;
        Debug.Log("Ball Thrown");
        ballDisplay.SetActive(false);
        isBallThrown = true;

        Transform ballHitPoint = SpawnPointProvider.Instance.BallHitPoint;
        BallController _ball = SingletonManager.NetworkingManager.InstantiateNetworkedObject(ballTemplate.name, ballDisplay.transform.position, Quaternion.identity).GetComponent<BallController>();
        _ball.OnThrow(ballHitPoint);
        StartCoroutine(C_WaitAfterBall());
    }

    private IEnumerator C_WaitAfterBall()
    {
        yield return new WaitForSeconds(0.7f);
        ResetTransforms();
    }

    private void ResetTransforms()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        isSelecting = true;
        isBallThrown = false;
        ballDisplay.SetActive(true);
    }
}
