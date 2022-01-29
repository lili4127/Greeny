using System.Collections;
using UnityEngine;
using TMPro;

public class EMPanel : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private TextMeshProUGUI[] waveNames;
    [SerializeField] private TextMeshProUGUI eMText;
    private Color32[] rainbowColors = new Color32[] {  Color.red, new Color(1, 0.5f, 0, 1), Color.yellow, Color.green, Color.blue, new Color(0.3f, 0, 0.5f, 1), new Color(0.6f, 0, 0.8f, 1) };
    private Color startColor = new Color(1, 1, 1, 0);

    private void Awake()
    {
        ResetTexts();
    }

    private void OnEnable()
    {
        StartCoroutine(FadeInTexts(duration));
    }

    private IEnumerator RainbowTextCo(TextMeshProUGUI t, float duration)
    {
        t.ForceMeshUpdate();
        TMP_TextInfo textInfo = t.textInfo;
        int characterCount = textInfo.characterCount;
        int currentCharacter = 0;
        bool going = true;
        Color32[] newVertexColors;

        while (going)
        {
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
                newVertexColors[vertexIndex + 0] = rainbowColors[i];
                newVertexColors[vertexIndex + 1] = rainbowColors[i];
                newVertexColors[vertexIndex + 2] = rainbowColors[i];
                newVertexColors[vertexIndex + 3] = rainbowColors[i];
                t.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                currentCharacter++;
                yield return new WaitForSeconds(0.15f);
            }
            going = false;
        }
    }

    IEnumerator FadeInTexts(float duration)
    {
        for(int i = 0; i < waveNames.Length; i++)
        {
            StartCoroutine(FadeInTextCo(waveNames[i], Color.white, 0.25f));
            yield return new WaitForSeconds(duration / waveNames.Length);
        }

        StartCoroutine(FadeInTextCo(eMText, Color.white, 0.5f));
    }

    IEnumerator FadeInTextCo(TextMeshProUGUI t, Color endValue, float duration)
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

    private void PulseText(TextMeshProUGUI t)
    {
        StartCoroutine(PulseTextCo(t));
    }

    IEnumerator PulseTextCo(TextMeshProUGUI t)
    {
        for (float i = 1f; i <= 1.1f; i += 0.005f)
        {
            t.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        t.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

        if(t.gameObject.name.Equals("VisibleText"))
        {
            StartCoroutine(RainbowTextCo(t, 1f));
        }        

        for (float i = 1.1f; i >= 1f; i -= 0.005f)
        {
            t.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        t.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ResetTexts()
    {
        foreach (TextMeshProUGUI t in waveNames)
        {
            t.color = startColor;
        }

        eMText.color = startColor;
    }

    private void OnDisable()
    {
        ResetTexts();
        StopAllCoroutines();
    }
}
