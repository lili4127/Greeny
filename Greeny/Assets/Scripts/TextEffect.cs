using System.Collections;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    public Color transparent = new Color(1, 1, 1, 0);

    public IEnumerator FadeInTextCo(TextMeshProUGUI t, Color endValue, float duration)
    {
        float time = 0;
        Color startValue = t.color;

        while (time < duration)
        {
            t.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        t.color = endValue;
        PulseText(t);
    }

    public void PulseText(TextMeshProUGUI t)
    {
        StartCoroutine(PulseTextCo(t));
    }

    public IEnumerator PulseTextCo(TextMeshProUGUI t)
    {
        for (float i = 1f; i <= 1.1f; i += 0.005f)
        {
            t.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        t.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

        for (float i = 1.1f; i >= 1f; i -= 0.005f)
        {
            t.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        t.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
}
