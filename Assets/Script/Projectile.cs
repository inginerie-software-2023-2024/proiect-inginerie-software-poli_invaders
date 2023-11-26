using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Vector3 direction;
    public SpriteRenderer projectileSr;
    public Rigidbody2D rb;

    public bool canSpawn = true;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void Update() {
        if (canSpawn)
            this.transform.position += direction * (speed * Time.deltaTime);
        else
        {
            projectileSr.enabled = false;
            Destroy(gameObject);
        }
            
    }

    private void OnTriggerEnter2D(Collider2D hitInfo) {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null) 
        {
            Destroy(gameObject);
        }

        if (!Player.instance.playerSr.enabled) {
            canSpawn = false;
            
            Destroy(gameObject);

    }
}
}
