using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HorrorUI : MonoBehaviour
{
    public Image hpFill;
    public Image batteryFill;
    public TextMeshProUGUI statusText;

    [Range(0,1)] public float hp = 1f;
    [Range(0,1)] public float battery = 1f;

    void Update()
    {
        hpFill.fillAmount = hp;
        batteryFill.fillAmount = battery;
    }

    public void SetHidden(bool hidden)
    {
        if (hidden)
        {
            statusText.text = "HIDDEN";
            statusText.color = Color.gray;
        }
        else
        {
            statusText.text = "DETECTED";
            statusText.color = Color.red;
        }
    }
}
