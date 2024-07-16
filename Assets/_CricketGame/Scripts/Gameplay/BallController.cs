using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody rigidBody;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    

    public void OnThrow(Transform hitPoint)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = hitPoint.position;

        // Calculate the direction and magnitude to hit the point
        Vector3 direction = endPosition - startPosition;
        float distance = direction.magnitude;
        direction.Normalize();

        // Use physics equations to calculate the initial velocity required to hit the hitpoint
        float g = Physics.gravity.y;
        float time = Mathf.Sqrt(-2 * distance / g);

        Vector3 initialVelocity = direction  * distance*2 * time;

        // Apply the calculated initial velocity to the ball's Rigidbody
        rigidBody.velocity = initialVelocity;
    }
}
