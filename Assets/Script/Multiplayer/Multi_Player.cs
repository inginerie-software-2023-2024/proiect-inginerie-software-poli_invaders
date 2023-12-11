using Riptide;
using UnityEngine;

public enum PlayerActions : ushort
{
    gotHit = 1,
    died = 2,
}

public class Multi_Player : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool Is_Main;
    [SerializeField] private int Health;
    [SerializeField] private int HurtDuration;

    private float hurtTimer;
    public bool IsMain { get { return Is_Main; } }

    private SpriteRenderer playerSR;
    public Multi_Movement PlayerMovement { get; private set; }

    [Header("Sprites")]
    [SerializeField] private Sprite DefaultSprite;
    [SerializeField] private Sprite HurtSprite;
    [SerializeField] private Sprite PinkSprite;
    [SerializeField] private Sprite OrangeSprite;
    [SerializeField] private Sprite PurpleSprite;

    private void Start()
    {
        playerSR = gameObject.GetComponent<SpriteRenderer>();
        PlayerMovement = gameObject.GetComponent<Multi_Movement>();

        if (gameObject.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.gravityScale = 0.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsMain && collision.GetComponent<Enemy>() != null)
        {
            SendAction(PlayerActions.gotHit);
        }
    }

    public int GetHealth()
    {
        return Health;
    }

    private void GetHit()
    {
        Health -= 1;
        if (IsMain && Health <= 0)
        {
            SendAction(PlayerActions.died);
            Cursor.visible = true;
        }
        else
        {
            playerSR.sprite = HurtSprite;
            hurtTimer = HurtDuration;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            playerSR.sprite = PinkSprite;
        }

        if (Input.GetKey(KeyCode.M))
        {
            playerSR.sprite = OrangeSprite;
        }

        if (Input.GetKey(KeyCode.B))
        {
            playerSR.sprite = PurpleSprite;
        }

        if (hurtTimer > 0)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0)
            {
                playerSR.sprite = DefaultSprite;
            }
        }
    }

    private void SendAction(PlayerActions action)
    {
        Message playerAction = Message.Create(MessageSendMode.Reliable, ClientToServerId.playerAction);
        playerAction.AddUShort((ushort)action);

        NetworkManager.Singleton.Client.Send(playerAction);
    }

    public void HandleAction(ushort action)
    {
        switch (action)
        {
            case (ushort)PlayerActions.gotHit:
                {
                    GetHit();
                    break;
                }
            case (ushort)PlayerActions.died:
                {
                    Destroy(gameObject);
                    break;
                }
            default:
                {
                    Debug.LogError($"(PLAYER): Error; Cannot handle player action with id {action}.");
                    break;
                }
        }
    }
}
