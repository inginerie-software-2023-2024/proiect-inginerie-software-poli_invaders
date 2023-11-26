using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    private float health = 5f;
    public Vector3 direction;
    public Rigidbody2D rb;
    public DoubleFire _doubleFirePU;
    public ShieldPU _shieldPU;
    private System.Random rnd = new System.Random();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    public void SetSpeed(float speed)
    {
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Projectile projectile = hitInfo.GetComponent<Projectile>();
        if (projectile != null)
        {
            health -= 1;
            if (health <= 0)
            {
                // if enemy is dead, destroy it
                Destroy(gameObject);

                // Spawn powerups
                if (rnd.Next(0, 100) > 90)
                {
                    Instantiate(_doubleFirePU, this.transform.position, Quaternion.identity);
                }
                else if (rnd.Next(0, 100) < 10)
                {
                    Instantiate(_shieldPU, this.transform.position, Quaternion.identity);
                }

                // Add score
                if (Score.Instance != null)
                {
                    Score.Instance.AddScore(1);
                }
            }
        }
    }
}
