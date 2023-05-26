using UnityEngine;

public class RotationController : MonoBehaviour
{
    public Transform rotatingObj;
    public float rotationDelay = 2f;
    public bool isClockwise = true;
    public float rotationSpeed = 50f;

    private bool rotationStarted = false;

    private void Start()
    {
        Invoke("StartRotation", rotationDelay);
    }

    private void Update()
    {
        if (rotationStarted)
        {
            float rotationDirection = isClockwise ? 1f : -1f;
            rotatingObj.Rotate(Vector3.up * (rotationDirection * rotationSpeed * Time.deltaTime));
        }
    }

    private void StartRotation()
    {
        rotationStarted = true;
    }
}