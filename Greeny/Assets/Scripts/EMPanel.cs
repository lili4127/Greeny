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
    private Color32[] rainbowColors = new Color32[] {  Color.red, new Color(1, 0.5f, 0, 1), Color.yellow, Color.green, Color.blue, new Color(0.3f, 0, 0.5f, 1), new Color(0.6f, 0, 0.8f, 1) };
    private Color startColor = new Color(1, 1, 1, 0);

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

    private void ResetPanel()
    {
        foreach (TextMeshProUGUI t in waveNames)
        {
            t.color = startColor;
        }

        eMText.color = startColor;
        eMImage.fillAmount = 0;
    }

    private void OnDisable()
    {
        ResetPanel();
        StopAllCoroutines();
    }
}
