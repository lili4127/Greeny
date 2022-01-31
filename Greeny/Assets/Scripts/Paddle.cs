using System.Collections;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Sprite ice;
    [SerializeField] private Sprite water;
    public int activePaddle { get; private set; }
    private SpriteRenderer spriteRenerer;

    private void Awake()
    {
        spriteRenerer = GetComponent<SpriteRenderer>();
        spriteRenerer.sprite = ice;
        activePaddle = 0;
    }

    private void OnEnable()
    {
        GameManager.changePaddle += UpdatePaddle;
    }

    private void OnDisable()
    {
        GameManager.changePaddle -= UpdatePaddle;
    }

    private void UpdatePaddle(int i)
    {

        if (i == 0)
        {
            spriteRenerer.sprite = ice;
            return;
        }

        else
        {
            StartCoroutine(PaddleCo(spriteRenerer, new Color(1, 1, 1, 0), 2f));
        }
    }

    IEnumerator PaddleCo(SpriteRenderer image, Color endValue, float duration)
    {
        bool ice = true;

        while (ice)
        {
            for (float i = 1; i >= 0f; i -= 0.005f)
            {
                image.color = new Color(1, 1, 1, i);
                yield return new WaitForEndOfFrame();
            }
            image.color = new Color(1, 1, 1, 0);
            image.sprite = water;
            activePaddle = 1;

            for (float i = 0f; i <= 1f; i += 0.005f)
            {
                image.color = new Color(1, 1, 1, i);
                yield return new WaitForEndOfFrame();
            }

            image.color = Color.white;
            ice = false;
        }
    }
}
