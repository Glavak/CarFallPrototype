using UnityEngine;
using UnityEngine.UI;

public class CarStats : MonoBehaviour
{
    public CarControls CarControls;

    private Text componentText;

    private void Start()
    {
        componentText = GetComponent<Text>();
    }

    private void Update()
    {
        componentText.text = Mathf.Round(CarControls.CurrentSpeed) + " m/s";
    }
}
