using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform ClosePosition;
    public float CloseFov = 55;

    public float FarSpeed = 20;
    public Transform FarPosition;
    public float FarFov = 70;

    public CarControls CarControls;
    public ParticleSystem ExhaustSparksA;
    public ParticleSystem ExhaustSparksB;
    private Camera mainCamera;

    private Vector3 shakeRotations;
    private float timeSinceShaked;
    private float speedLastFrame;
    private float speedNormalized;

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        speedNormalized = Mathf.Lerp(speedNormalized,
            CarControls.CurrentSpeed / FarSpeed + CarControls.MotorInput * .4f, Time.deltaTime);

        PositionCamera();
        GearShiftAnimation();
    }

    private void PositionCamera()
    {
        mainCamera.transform.position =
            Vector3.LerpUnclamped(ClosePosition.position, FarPosition.position, speedNormalized);

        mainCamera.transform.rotation = Quaternion.Slerp(ClosePosition.rotation, FarPosition.rotation, speedNormalized);

        float targetFov = CarControls.IsGrounded
            ? Mathf.LerpUnclamped(CloseFov, FarFov, speedNormalized)
            : 60f;
        mainCamera.fieldOfView = Mathf.MoveTowards(mainCamera.fieldOfView, targetFov, Time.deltaTime);

        transform.position = CarControls.transform.position;

        Quaternion targetRotation;
        if (CarControls.IsGrounded)
        {
            targetRotation = CarControls.transform.rotation;
            targetRotation *= Quaternion.Euler(shakeRotations * speedNormalized * 1.6f);
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

    private void GearShiftAnimation()
    {
        if (CarControls.IsGrounded && (int) speedLastFrame / 7 < (int) CarControls.CurrentSpeed / 7)
        {
            speedNormalized -= 0.02f;
            ExhaustSparksA.Play();
            ExhaustSparksB.Play();
        }

        speedLastFrame = CarControls.CurrentSpeed;
    }
}