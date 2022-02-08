using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BallPool ballPool;
    [SerializeField] Slider healthbar;
    public int worldHealth { get; private set; }
    public bool gamePlaying { get; private set; }
    private bool isPaused = false;

    [Space]
    [Header("Backgrounds")]
    [SerializeField] private int activeBackground;
    [SerializeField] private SpriteRenderer[] backgrounds;
    private Color targetColor = new Color(1, 1, 1, 0);

    [Header("Canvas")]
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI countdownText;

    public static event System.Action<int> changePaddle;

    private void OnEnable()
    {
        Ball.ballAbsorbed += UpdateCounter;
    }

    private void Start()
    {
        playPanel.SetActive(true);
    }

    private void SetUpGame()
    {
        //set background opacities to 1
        foreach (SpriteRenderer i in backgrounds)
        {
            i.color = Color.white;
        }
        CleanBallsOffScreen();
        activeBackground = 0;
        worldHealth = 20;
        healthbar.value = worldHealth;
        changePaddle?.Invoke(0);
    }

    private void StartGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        int countdownTime = 3;
        countdownText.text = countdownTime.ToString();
        countdownText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);

        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        HUD.SetActive(true);
        gamePlaying = true;
        StartCoroutine(SunEmit());
        StartCoroutine(PlanetEmit());
    }

    IEnumerator SunEmit()
    {
        while (gamePlaying)
        {
            Ball b = ballPool.Get();
            b.gameObject.SetActive(true);
            b.SunEmit();
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator PlanetEmit()
    {
        while (gamePlaying)
        {
            List<int> planetPos = new List<int>() { 0, 1, 2 };
            int r = planetPos[Random.Range(0, planetPos.Count)];
            Ball b = ballPool.Get();
            b.gameObject.SetActive(true);
            b.PlanetEmit(r);
            planetPos.Remove(r);

            if (worldHealth >= 45)
            {
                int r2 = planetPos[Random.Range(0, planetPos.Count)];
                Ball b2 = ballPool.Get();
                b2.gameObject.SetActive(true);
                b2.PlanetEmit(r2);
                planetPos.Remove(r2);
            }

            if (worldHealth >= 70)
            {
                int r3 = planetPos[0];
                Ball b3 = ballPool.Get();
                b3.gameObject.SetActive(true);
                b3.PlanetEmit(r3);

            }

            yield return new WaitForSeconds(5f);
        }
    }

    private void UpdateCounter()
    {
        if (gamePlaying)
        {
            worldHealth++;
            healthbar.value = worldHealth;

            if(worldHealth == 60)
            {
                changePaddle?.Invoke(1);
            }

            if (worldHealth % 20 == 0 && activeBackground <= 3)
            {
                if (activeBackground == 3)
                {
                    LoseGame();
                }

                else
                {
                    StartCoroutine(BackgroundCo(backgrounds[activeBackground], targetColor, 5));
                    activeBackground++;
                }
            }
        }
    }

    IEnumerator BackgroundCo(SpriteRenderer imageToFade, Color endValue, float duration)
    {
        float time = 0;
        Color startValue = imageToFade.color;

        while (time < duration)
        {
            imageToFade.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        imageToFade.color = endValue;
    }

    private void LoseGame()
    {
        HUD.SetActive(false);
        CleanBallsOffScreen();
        gamePlaying = false;
        gameOverPanel.SetActive(true);
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        HUD.SetActive(!isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
            gamePlaying = false;
            pausePanel.SetActive(true);
        }

        else
        {
            Time.timeScale = 1f;
            gamePlaying = true;
            pausePanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        playPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        StopAllCoroutines();
        SetUpGame();
        StartGame();
    }

    private void CleanBallsOffScreen()
    {
        foreach (Transform t in ballPool.transform)
        {
            t.GetComponent<Ball>().ResetBall();
        }
    }

    private void OnDisable()
    {
        Ball.ballAbsorbed -= UpdateCounter;
    }
}
