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
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFov, Time.deltaTime);

        transform.position = CarControls.transform.position;

        Debug.Log(CarControls.IsGrounded);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            CarControls.IsGrounded ? CarControls.transform.rotation : Quaternion.identity,
            Time.deltaTime
        );
    }
}