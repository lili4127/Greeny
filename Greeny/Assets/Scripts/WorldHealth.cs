using UnityEngine;
using UnityEngine.UI;


public class WorldHealth : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient gradient;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        fillImage.color = gradient.Evaluate(slider.normalizedValue);
        Ball.ballAbsorbed += UpdateBar;
    }

    private void OnDisable()
    {
        Ball.ballAbsorbed -= UpdateBar;
    }

    private void UpdateBar()
    {
        fillImage.color = gradient.Evaluate(slider.normalizedValue);
    }
}
