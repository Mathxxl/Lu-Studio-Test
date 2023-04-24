using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotationSlider : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private TextMeshProUGUI valueText;
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void UpdateIndicator()
    {
        var value = 0f;
        if (_slider != null)
        {
            value = _slider.value;
        }

        value = Mathf.Round(value * 10f) / 10f;

        PlayerBall pBall = playerTransform.GetComponent<PlayerBall>();
        var diff = pBall.ShotAngle - value;
        pBall.ShotAngle = value;

        valueText.text = value + "°";

        indicator.transform.RotateAround(playerTransform.position, Vector3.forward, diff);
    }
}
