using UnityEngine;

public enum Position
{
    LeftDown,
    RightDown,
    MiddleDown,
    LeftUp,
    RightUp,
    MiddleUp
}

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private BallPool bp;
    private int activeColor;

    //positions
    private Vector3 sun;
    private Vector3 planetMid;
    private Vector3 planetLeft;
    private Vector3 planetRight;

    //ball sprites
    [SerializeField] private Sprite white;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite blue;
    [SerializeField] private Sprite red;
    private SpriteRenderer spriteRenderer;

    //events
    public static event System.Action ballAbsorbed;
    public static event System.Action ballBounced;
    public static event System.Action ballHitGas;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bp = GetComponentInParent<BallPool>();
        sun = transform.parent.transform.position;
        planetMid = new Vector3(0, -sun.y, 0);
        planetLeft = new Vector3(-14, -sun.y, 0);
        planetRight = new Vector3(14, -sun.y, 0);
    }

    private void SetRandomColor()
    {
        int r = Random.Range(0, 4);

        switch (r)
        {
            case 0:
                spriteRenderer.sprite = white;
                break;
            case 1:
                spriteRenderer.sprite = yellow;
                break;
            case 2:
                spriteRenderer.sprite = green;
                break;
            case 3:
                spriteRenderer.sprite = blue;
                break;
            default:
                break;
        }
        activeColor = r;
    }

    public void SunEmit()
    {
        SetRandomColor();
        transform.position = sun;
        rb.velocity = new Vector2(Random.Range(-7, 8), -8);
    }

    public void PlanetEmit(int i)
    {
        switch (i)
        {
            case 0:
                transform.position = planetMid;
                break;
            case 1:
                transform.position = planetLeft;
                break;
            case 2:
                transform.position = planetRight;
                break;
            default:
                break;
        }

        spriteRenderer.sprite = red;
        activeColor = 4;
        rb.velocity = new Vector2(0, 5);
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
            ResetBall();
        }

        if (collision.TryGetComponent<Paddle>(out Paddle p))
        {
            if (activeColor < 4)
            {
                if (p.activePaddle == 0)
                {
                    rb.velocity = new Vector2(0, 15);
                }

                else
                {
                    rb.velocity = new Vector2(0, 5);
                }
            }

            else
            {
                ballAbsorbed?.Invoke();
                ResetBall();
            }
        }

        if (collision.TryGetComponent<GreenhouseGas>(out GreenhouseGas g))
        {
            if (rb.velocity.y < 0)
            {
                return;
            }

            if (!g.gasActive && activeColor == 4)
            {
                ResetBall();
                ballHitGas?.Invoke();
            }
        }
    }

    public void ServeGasBall(Position p)
    {
        spriteRenderer.sprite = red;
        activeColor = 4;

        switch (p)
        {
            case Position.LeftDown:
                rb.velocity = new Vector2(-1f, -1f);
                break;
            case Position.RightDown:
                rb.velocity = new Vector2(1f, -1f);
                break;
            case Position.MiddleDown:
                rb.velocity = new Vector2(0f, -1f);
                break;
            case Position.LeftUp:
                rb.velocity = new Vector2(-1f, 1f);
                break;
            case Position.RightUp:
                rb.velocity = new Vector2(1f, 1f);
                break;
            case Position.MiddleUp:
                rb.velocity = new Vector2(0f, 1f);
                break;
            default:
                break;
        }

        rb.velocity *= 3;
    }

    public void ResetBall()
    {
        this.gameObject.SetActive(false);
        transform.position = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
        rb.velocity = Vector2.zero;
        bp.ReturnToPool(this);
    }
}
