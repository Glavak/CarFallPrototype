using UnityEngine;
using UnityEngine.UI;

public class CarStats : MonoBehaviour
{
    public CarControls CarControls;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        GetComponent<Text>().text = Mathf.Round(CarControls.CurrentSpeed) + "m/s\n";
    }
}
