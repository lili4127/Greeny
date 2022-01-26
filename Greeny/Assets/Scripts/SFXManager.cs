using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Space]
    [Header("SFX")]
    [SerializeField] private AudioClip ballSun;
    [SerializeField] private AudioClip ballBounce;
    [SerializeField] private AudioClip gasLaugh;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Ball.ballSun += PlayBallSun;
        Ball.ballBounced += PlayBallBounce;
        GreenhouseGas.gasLaugh += PlayGasLaugh;
    }

    private void PlayBallBounce()
    {
        audioSource.PlayOneShot(ballBounce, 0.25f);
    }

    private void PlayBallSun()
    {
        audioSource.PlayOneShot(ballSun, 0.25f);
    }

    private void PlayGasLaugh()
    {
        audioSource.PlayOneShot(gasLaugh, 0.25f);
    }

    private void OnDisable()
    {
        Ball.ballSun -= PlayBallSun;
        Ball.ballBounced -= PlayBallBounce;
        GreenhouseGas.gasLaugh -= PlayGasLaugh;
    }
}
