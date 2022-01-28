using System.Collections;
using UnityEngine;
using TMPro;

public class EMPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] texts;
    private Color[] colors = new Color[] {Color.red, Color.yellow, Color.green, Color.white, Color.black, Color.magenta, Color.cyan, Color.white };
    private Color32[] rainbowColors = new Color32[] {  Color.red, new Color(1, 0.5f, 0, 1), Color.yellow, Color.green, Color.blue, new Color(0.3f, 0, 0.5f, 1), new Color(0.6f, 0, 0.8f, 1) };
    private Color startColor = new Color(1, 1, 1, 0);

    private LineRenderer lineRenderer;
    [SerializeField] private int amplitude;

    //lower makes longer wavelengths
    [SerializeField] private float tauModifier = 0.75f;

    private void Awake()
    {
        ResetTexts();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void OnEnable()
    {

        StartCoroutine(DrawWaveCo(5f));

        //draw line and at each interval fade in text;
        for(int i = 0; i < texts.Length; i++)
        {
            StartCoroutine(FadeInTextCo(texts[i], Color.white, 1.5f, i));
        }
    }

    IEnumerator DrawWaveCo(float duration)
    {
        float xStart = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        float xFinish = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        lineRenderer.positionCount = 0;
        float time = 0f;

        while(time < duration)
        {
            lineRenderer.positionCount++;
            float x = Mathf.Lerp(xStart, xFinish, time/duration);
            float y = amplitude * Mathf.Cos(tauModifier * Mathf.PI * x);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(x, y + 3, 0));
            time += Time.deltaTime;
            yield return null;
        }
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

    IEnumerator FadeInTextCo(TextMeshProUGUI t, Color endValue, float duration, int index)
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
        PulseText(t, index);
    }

    private void PulseText(TextMeshProUGUI t, int index)
    {
        StartCoroutine(PulseTextCo(t, index));
    }

    IEnumerator PulseTextCo(TextMeshProUGUI t, int index)
    {
        for (float i = 1f; i <= 1.1f; i += 0.005f)
        {
            t.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        t.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        t.color = colors[index];
        if (index == 3)
        {
            StartCoroutine(RainbowTextCo(t, 1.5f));
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
        foreach (TextMeshProUGUI t in texts)
        {
            t.color = startColor;
        }
    }

    private void OnDisable()
    {
        ResetTexts();
        StopAllCoroutines();
        lineRenderer.positionCount = 0;
    }
}
