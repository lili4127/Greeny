using UnityEngine;

public enum Position
{
    Left,
    Right,
    Middle
}

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private BallPool bp;

    [SerializeField] private Sprite[] ballSprites;
    private SpriteRenderer spriteRenderer;

    public static event System.Action ballAbsorbed;
    public static event System.Action ballBounced;
    public static event System.Action ballHitGas;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bp = GetComponentInParent<BallPool>();
    }

    private void SetRandomColor()
    {
        int r = Random.Range(0, ballSprites.Length);
        spriteRenderer.sprite = ballSprites[r];
    }

    private void SetColor(int c)
    {
        spriteRenderer.sprite = ballSprites[c];
    }

    public void Serve()
    {
        SetRandomColor();
        rb.velocity = new Vector2(Random.Range(-7, 8), -8);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SideWall>(out SideWall s))
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            ballBounced?.Invoke();
        }

        if (collision.TryGetComponent<TopWall>(out TopWall t))
        {
            ResetBall();
        }

        if (collision.TryGetComponent<BottomWall>(out BottomWall b))
        {
            ballAbsorbed?.Invoke();
            BallAbsorbed();
        }

        if (collision.TryGetComponent<GreenhouseGas>(out GreenhouseGas g))
        {
            if (rb.velocity.y < 0)
            {
                return;
            }

            if (!g.gasActive)
            {
                ResetBall();
                ballHitGas?.Invoke();
            }
        }
    }

    private void BallAbsorbed()
    {
        rb.velocity = Vector2.zero;
        this.gameObject.SetActive(false);
        Invoke(nameof(EmitRadiation), 0.25f);
    }

    private void EmitRadiation()
    {
        SetColor(4);
        this.gameObject.SetActive(true);
        rb.velocity = new Vector2(0, 8);
    }

    public void ServeGasBall(Position p)
    {
        SetColor(4);

        switch (p)
        {
            case Position.Left:
                rb.velocity = new Vector2(1f, -1f);
                break;
            case Position.Middle:
                rb.velocity = new Vector2(0f, -1f);
                break;
            case Position.Right:
                rb.velocity = new Vector2(-1f, -1f);
                break;
            default:
                break;
        }

        rb.velocity *= Random.Range(2, 4);
    }

    public void ResetBall()
    {
        this.gameObject.SetActive(false);
        transform.position = transform.parent.transform.position;
        rb.velocity = Vector2.zero;
        bp.ReturnToPool(this);
    }
}
