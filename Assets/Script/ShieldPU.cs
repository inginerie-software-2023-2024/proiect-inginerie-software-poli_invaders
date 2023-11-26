using UnityEngine;

public class ShieldPU : MonoBehaviour
{
    public Rigidbody2D rb;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    
    private void OnTriggerEnter2D(Collider2D hitInfo) 
    {
        Player player = hitInfo.GetComponent<Player>();
        if (player != null) {
            Destroy(gameObject);
        }
    }
}
