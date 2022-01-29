using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Space]
    [Header("SFX")]
    [SerializeField] private AudioClip ballAbsorbed;
    [SerializeField] private AudioClip ballBounce;
    [SerializeField] private AudioClip gasLaugh;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Ball.ballAbsorbed += PlayBallAbsorbed;
        Ball.ballBounced += PlayBallBounce;
        GreenhouseGas.gasLaugh += PlayGasLaugh;
    }

    private void PlayBallBounce()
    {
        audioSource.PlayOneShot(ballBounce, 0.25f);
    }

    private void PlayBallAbsorbed()
    {
        audioSource.PlayOneShot(ballAbsorbed, 0.25f);
    }

    private void PlayGasLaugh()
    {
        audioSource.PlayOneShot(gasLaugh, 0.25f);
    }

    private void OnDisable()
    {
        Ball.ballAbsorbed -= PlayBallAbsorbed;
        Ball.ballBounced -= PlayBallBounce;
        GreenhouseGas.gasLaugh -= PlayGasLaugh;
    }
}
