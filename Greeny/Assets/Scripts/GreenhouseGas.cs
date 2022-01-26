using System;
using UnityEngine;

public class GreenhouseGas : MonoBehaviour
{
    [SerializeField] private BallPool ballPool;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float speed = 10f;
    private float movementInput;
    public bool gasActive;
    public static event Action gasLaugh;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gasActive = false;
    }

    private void OnEnable()
    {
        Ball.ballHitGas += GasActivated;
    }

    private void Update()
    {
        movementInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (movementInput != 0 && !gasActive)
        {
            rb.MovePosition(transform.position + transform.right * movementInput * speed * Time.fixedDeltaTime);
        }
    }

    private void GasActivated()
    {
        anim.SetTrigger("gasActive");
        gasActive = true;
        Invoke(nameof(ReemitRadiation), 0.25f);
    }

    private void ResetGas()
    {
        gasActive = false;
        anim.ResetTrigger("gasActive");
    }

    private void ReemitRadiation()
    {
        gasLaugh?.Invoke();
        foreach (Position p in Enum.GetValues(typeof(Position)))
        {
            Ball b = ballPool.Get();
            b.transform.position = transform.position;
            b.gameObject.SetActive(true);
            b.ServeGasBall(p);
        }
        Invoke(nameof(ResetGas), 0.5f);
    }

    private void OnDisable()
    {
        Ball.ballHitGas -= GasActivated;
    }
}
