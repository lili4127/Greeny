using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SunPanel : MonoBehaviour
{
    [SerializeField] private TextEffect textEffect;
    [SerializeField] private TextMeshProUGUI sunText;
    [SerializeField] private Sprite[] ballSprites;
    [SerializeField] private Image ballImage;
    private Color startColor = new Color(1, 1, 1, 0);
    private int sprite;

    private void Awake()
    {
        ballImage.sprite = ballSprites[0];
        sprite = 0;
        sunText.color = startColor;
    }

    private void OnEnable()
    {
        StartCoroutine(textEffect.FadeInTextCo(sunText, Color.white, 2f));
    }

    public void ChangeSprite()
    {
        sprite++;

        if(sprite >= ballSprites.Length)
        {
            sprite = 0;
        }

        ballImage.sprite = ballSprites[sprite];
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        sunText.color = startColor;
    }
}
