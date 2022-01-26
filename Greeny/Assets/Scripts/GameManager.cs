using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BallPool ballPool;
    public bool gamePlaying { get; private set; }
    private bool isPaused = false;

    [Space]
    [Header("Backgrounds")]
    [SerializeField] private int playerCounter;
    [SerializeField] private int activeBackground;
    [SerializeField] private SpriteRenderer[] backgrounds;
    private Color targetColor = new Color(1, 1, 1, 0);

    [Header("Canvas")]
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI countdownText;

    private void OnEnable()
    {
        Ball.ballGas += UpdateCounter;
    }

    private void Start()
    {
        SetUpGame();
        StartGame();
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
        playerCounter = 0;
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
        pauseButton.SetActive(true);
        gamePlaying = true;
        StartCoroutine(ServeBalls());
    }

    IEnumerator ServeBalls()
    {
        while (gamePlaying)
        {
            Ball b = ballPool.Get();
            b.gameObject.SetActive(true);
            b.Serve();
            
            yield return new WaitForSeconds(3f);
        }
    }

    private void UpdateCounter()
    {
        if (gamePlaying)
        {
            playerCounter++;

            if (playerCounter % 5 == 0 && activeBackground <= 3)
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
        pauseButton.SetActive(false);
        CleanBallsOffScreen();
        gamePlaying = false;
        gameOverPanel.SetActive(true);
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        pauseButton.SetActive(!isPaused);

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
        Ball.ballGas -= UpdateCounter;
    }
}
