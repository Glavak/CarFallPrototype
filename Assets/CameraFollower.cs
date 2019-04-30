using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform ClosePosition;
    public float CloseFov = 55;

    public float FarSpeed = 20;
    public Transform FarPosition;
    public float FarFov = 70;

    public CarControls CarControls;
    private Camera mainCamera;

    private Vector3 shakeRotations;
    private float timeSinceShaked;

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        mainCamera.transform.position = Vector3.LerpUnclamped(ClosePosition.position, FarPosition.position,
            CarControls.CurrentSpeed / FarSpeed);
        mainCamera.transform.rotation = Quaternion.Slerp(ClosePosition.rotation, FarPosition.rotation,
            CarControls.CurrentSpeed / FarSpeed);

        float targetFov = CarControls.IsGrounded
            ? Mathf.LerpUnclamped(CloseFov, FarFov, CarControls.CurrentSpeed / FarSpeed)
            : 60f;
        mainCamera.fieldOfView = Mathf.MoveTowards(mainCamera.fieldOfView, targetFov, Time.deltaTime);

        transform.position = CarControls.transform.position;

        Quaternion targetRotation;
        if (CarControls.IsGrounded)
        {
            targetRotation = CarControls.transform.rotation;
            targetRotation *= Quaternion.Euler(shakeRotations * CarControls.CurrentSpeed / FarSpeed);
        }
        else
        {
            targetRotation = Quaternion.Euler(30, 0, 0);
            targetRotation *= Quaternion.Euler(shakeRotations);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1.1f);
        
        if (timeSinceShaked > .1f)
        {
            shakeRotations = Random.insideUnitSphere.normalized;
            timeSinceShaked -= .1f;
        }
        else
        {
            timeSinceShaked += Time.fixedDeltaTime;
        }
    }
}