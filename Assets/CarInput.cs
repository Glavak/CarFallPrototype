using UnityEngine;

[RequireComponent(typeof(CarControls))]
public class CarInput : MonoBehaviour
{
    private CarControls carController;

    private void Start()
    {
        carController = GetComponent<CarControls>();
    }

    private void Update()
    {
        carController.MotorInput = Input.GetMouseButton(0) ? 1 : 0;
    }
}
