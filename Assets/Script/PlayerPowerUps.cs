using UnityEngine;
using System.Collections;

public class PlayerPowerUps : MonoBehaviour
{
    public bool doubleFire = false;
    public bool shieldActive = false; // Indicates if the shield is active
    public Projectile laserPrefab;
    public SpriteRenderer playerSprite;
    public Sprite shieldSprite;
    public Sprite defaultSprite;
    public float doubleFireDuration = 10f; // Duration of the double fire power-up
    public float shieldDuration = 5f; // Duration of the shield power-up
    public Collider2D playerCollider; // Reference to the player's collider component


    public void Awake()
    {
        // Get the player's collider component
        playerCollider = GetComponent<Collider2D>();
    }
    public void Update()
    {
        // Shoot when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0) && doubleFire)
        {
            PerformDoubleShoot();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            PerformShoot();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DoubleFirePU"))
        {
            ActivateDoubleFirePowerUp();
            Debug.Log(doubleFire);
        }
        else if (other.gameObject.CompareTag("ShieldPU"))
        {
            ActivateShieldPowerUp();
            Debug.Log("Shield activated");
        }
    }

    public void ActivateDoubleFirePowerUp()
    {
        doubleFire = true;
        StartCoroutine(ResetDoubleFirePowerUpCoroutine());
    }

    public void ActivateShieldPowerUp()
    {
        if (!shieldActive)
        {
            shieldActive = true;

            if (playerSprite != null)
                playerSprite.sprite = shieldSprite; // Change the player sprite to the shield sprite

            // Ignore collisions with enemies
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

            StartCoroutine(ResetShieldPowerUpCoroutine());
        }
    }

    public IEnumerator ResetShieldPowerUpCoroutine()
    {
        yield return new WaitForSeconds(shieldDuration);

        shieldActive = false;
        playerSprite.sprite = defaultSprite; // Change the player sprite back to the default sprite

        // Revert collisions with enemies
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    public IEnumerator ResetDoubleFirePowerUpCoroutine()
    {
        yield return new WaitForSeconds(doubleFireDuration);
        doubleFire = false;
    }

    public void PerformShoot()
    {
        Vector3 laserOffset = new Vector3(0.99f, 0.36f, 0);
        Vector3 spawnPosition = transform.position + laserOffset;
        Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
    }

    public void PerformDoubleShoot()
    {
        Vector3[] laserOffsets = { new Vector3(0.99f, 0.43f, 0), new Vector3(0.99f, 0.27f, 0) };

        foreach (Vector3 offset in laserOffsets)
        {
            Vector3 spawnPosition = transform.position + offset;
            Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
