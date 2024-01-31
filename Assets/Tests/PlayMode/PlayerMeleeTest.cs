using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MeleeTest
{
    [UnityTest]
    public IEnumerator Attack_DamagesEnemiesInRange()
    {
        // Arrange
        GameObject meleeObject = new GameObject();
        Melee melee = meleeObject.AddComponent<Melee>();
        GameObject enemyObject = new GameObject();
        enemyObject.AddComponent<Enemy>();
        enemyObject.transform.position = melee.attackPoint.position;
        Collider2D enemyCollider = enemyObject.GetComponent<Collider2D>();
        melee.enemyLayers = LayerMask.GetMask("Enemy");
        melee.damage = 10f;

        // Act
        melee.Attack();

        // Assert
        yield return null; // Wait for the end of frame
        Assert.AreEqual(90f, enemyObject.GetComponent<Enemy>().health); // Assuming Enemy class has a health property
    }

    [UnityTest]
    public IEnumerator ActivateDoubleFirePowerUp_IncreasesRangeAndSize()
    {
        // Arrange
        GameObject meleeObject = new GameObject();
        Melee melee = meleeObject.AddComponent<Melee>();
        melee.range = 1f;
        melee.attackPointSprite = melee.attackPoint.GetComponent<SpriteRenderer>();
        Vector3 initialSize = melee.attackPointSprite.transform.localScale;

        // Act
        melee.ActivateDoubleFirePowerUp();

        // Assert
        yield return null; // Wait for the end of frame
        Assert.AreEqual(3f, melee.range);
        Assert.AreEqual(initialSize * 3, melee.attackPointSprite.transform.localScale);
    }

    [UnityTest]
    public IEnumerator ActivateShieldPowerUp_ChangesPlayerSpriteAndIgnoresEnemyCollisions()
    {
        // Arrange
        GameObject meleeObject = new GameObject();
        Melee melee = meleeObject.AddComponent<Melee>();
        GameObject playerObject = new GameObject();
        SpriteRenderer playerSpriteRenderer = playerObject.AddComponent<SpriteRenderer>();
        melee.playerSprite = playerSpriteRenderer;
        melee.shieldZprite = Resources.Load<Sprite>("ShieldSprite"); // Assuming there is a shield sprite in the Resources folder
        melee.shieldDuration = 5f;

        // Act
        melee.ActivateShieldPowerUp();

        // Assert
        yield return new WaitForSeconds(0.1f); // Wait for the attack animation to finish
        Assert.AreEqual(melee.shieldZprite, playerSpriteRenderer.sprite);
        Assert.IsTrue(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy")));
        yield return new WaitForSeconds(melee.shieldDuration); // Wait for the shield duration to finish
        Assert.IsFalse(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy")));
    }
}