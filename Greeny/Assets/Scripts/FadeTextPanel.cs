using UnityEngine;
using TMPro;

public class FadeTextPanel : MonoBehaviour
{
    [SerializeField] private TextEffect textEffect;
    private TextMeshProUGUI activeText;

    private void Awake()
    {
        activeText = GetComponentInChildren<TextMeshProUGUI>();
        activeText.color = textEffect.transparent;
    }

    private void OnEnable()
    {
        activeText.color = textEffect.transparent;
        StartCoroutine(textEffect.FadeInTextCo(activeText, Color.white, 1.5f));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        activeText.color = textEffect.transparent;
    }
}
