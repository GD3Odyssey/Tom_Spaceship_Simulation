using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FuelUI : MonoBehaviour
{
    public TextMeshProUGUI fuelText;
    public Scrollbar fuelScrollbar;
    public int decimalPlaces = 2;

    private void Start()
    {
        fuelText.text = GameManager.instance.fuel.ToString("F0");
        fuelScrollbar.size = GameManager.instance.fuel / 1000f;
    }

    void Update()
    {
        fuelText.text = GameManager.instance.fuel.ToString("F0");
        fuelScrollbar.size = GameManager.instance.fuel / 1000f;

        if (fuelText != null)
        {
            fuelText.text = GameManager.instance.fuel.ToString("F" + decimalPlaces);
        }
    }
}
