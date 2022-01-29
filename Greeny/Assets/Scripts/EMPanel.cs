using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EMPanel : MonoBehaviour
{
    [SerializeField] private TextEffect textEffect;
    [SerializeField] private float duration = 5f;
    [SerializeField] private TextMeshProUGUI[] waveNames;
    [SerializeField] private TextMeshProUGUI eMText;
    [SerializeField] private Image eMImage;

    private void Awake()
    {
        ResetPanel();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeInImage(duration));
        StartCoroutine(FadeInTexts(duration));
    }

    IEnumerator FadeInImage(float duration)
    {
        float time = 0;

        while (time < duration)
        {
            eMImage.fillAmount = Mathf.Lerp(0, 1, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        eMImage.fillAmount = 1;
    }

    IEnumerator FadeInTexts(float duration)
    {
        for(int i = 0; i < waveNames.Length; i++)
        {
            StartCoroutine(textEffect.FadeInTextCo(waveNames[i], Color.white, 0.25f));
            yield return new WaitForSeconds(duration / waveNames.Length);
        }

        StartCoroutine(textEffect.FadeInTextCo(eMText, Color.white, 0.5f));
    }

    private void ResetPanel()
    {
        foreach (TextMeshProUGUI t in waveNames)
        {
            t.color = textEffect.transparent;
        }

        eMText.color = textEffect.transparent;
        eMImage.fillAmount = 0;
    }

    private void OnDisable()
    {
        ResetPanel();
        StopAllCoroutines();
    }
}
