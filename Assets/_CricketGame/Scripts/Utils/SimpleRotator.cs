using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis;

    private void Update()
    {
        transform.Rotate(rotationAxis * Time.deltaTime);
    }

}
