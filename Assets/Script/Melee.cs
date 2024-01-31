using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour
{
    public float damage = 10f;
    public float range = 1.3f;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public SpriteRenderer playerSprite;
    public SpriteRenderer attackPointSprite;
    public Sprite defaultSprite;
    public float doubleFireDuration = 10f; // Duration of the double fire power-up
    public float shieldDuration = 5f; // Duration of the shield power-up
    private Collider2D playerCollider; // Reference to the player's collider component
    private bool doubleFire = false;
    private bool shieldActive = false;
    public Sprite shieldZprite;
    private Vector3 initialSize;

    private void Awake()
    {
        attackPointSprite = attackPoint.GetComponent<SpriteRenderer>();
        initialSize = attackPointSprite.transform.localScale;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            AttackAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DoubleFirePU"))
        {
            ActivateDoubleFirePowerUp();
            Debug.Log("Double fire activated");
        }
        else if (other.gameObject.CompareTag("ShieldPU"))
        {
            ActivateShieldPowerUp();
            Debug.Log("Shield activated");
        }
    }

    void Attack()
    {
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, range, enemyLayers);

        // Damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private void AttackAnimation()
    {
        attackPointSprite = attackPoint.GetComponent<SpriteRenderer>();
        if (attackPointSprite != null)
        {
            float alpha = 100f;
            Color color = attackPointSprite.color;
            color.a = alpha;
            attackPointSprite.color = color;
            StartCoroutine(ResetAttackAnimationCoroutine());
        }
    }

    private IEnumerator ResetAttackAnimationCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        float alpha = 0f;
        Color color = attackPointSprite.color;
        color.a = alpha;
        attackPointSprite.color = color;
    }

    // helper method to visualize the attack range
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, range);
    }

    private void ActivateDoubleFirePowerUp()
    {
        range *= 3f;
        doubleFire = true;
        attackPointSprite.transform.localScale *= 3;
        StartCoroutine(ResetDoubleFirePowerUpCoroutine());
    }

    private void ActivateShieldPowerUp()
    {
        if (!shieldActive)
        {
            shieldActive = true;
            playerSprite.sprite = shieldZprite; // Change the player sprite to the shield sprite

            // Ignore collisions with enemies
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

            StartCoroutine(ResetShieldPowerUpCoroutine());
        }
    }

    private IEnumerator ResetShieldPowerUpCoroutine()
    {
        yield return new WaitForSeconds(shieldDuration);

        shieldActive = false;
        playerSprite.sprite = defaultSprite; // Change the player sprite back to the default sprite

        // Revert collisions with enemies
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    private IEnumerator ResetDoubleFirePowerUpCoroutine()
    {
        yield return new WaitForSeconds(doubleFireDuration);
        range = 1f;
        attackPointSprite.transform.localScale = initialSize;
        doubleFire = false;
    }
}