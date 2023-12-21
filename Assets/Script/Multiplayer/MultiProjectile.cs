using UnityEngine;

public class MultiProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    
    public bool CanSpawn { get; private set; }
    public Multi_Player player { get; private set; }

    private SpriteRenderer projectileSR;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    private void Start()
    {
        projectileSR = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0.0f;
        CanSpawn = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (CanSpawn)
        {
            transform.position += direction * (speed * Time.deltaTime);
        }
        else
        {
            projectileSR.enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.TryGetComponent(out MultiEnemy _))
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayer(Multi_Player player)
    {
        this.player = player;
    }
}
