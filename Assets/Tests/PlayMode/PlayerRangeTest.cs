using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerPowerUpsTests
{
    [UnityTest]
    public IEnumerator DoubleFirePowerUp_ActivatesCorrectly()
    {
        // Arrange
        GameObject player = new GameObject();
        PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
        GameObject doubleFirePowerUp = new GameObject();
        doubleFirePowerUp.tag = "DoubleFirePU";

        // Act
        playerPowerUps.OnTriggerEnter2D(doubleFirePowerUp.GetComponent<Collider2D>());

        // Assert
        yield return null; // Wait for the end of frame
        Assert.IsTrue(playerPowerUps.IsDoubleFireActive());
    }

    [UnityTest]
    public IEnumerator ShieldPowerUp_ActivatesCorrectly()
    {
        // Arrange
        GameObject player = new GameObject();
        PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
        GameObject shieldPowerUp = new GameObject();
        shieldPowerUp.tag = "ShieldPU";

        // Act
        playerPowerUps.OnTriggerEnter2D(shieldPowerUp.GetComponent<Collider2D>());

        // Assert
        yield return null; // Wait for the end of frame
        Assert.IsTrue(playerPowerUps.IsShieldActive());
    }

    [UnityTest]
    public IEnumerator DoubleFirePowerUp_DeactivatesAfterDuration()
    {
        // Arrange
        GameObject player = new GameObject();
        PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
        GameObject doubleFirePowerUp = new GameObject();
        doubleFirePowerUp.tag = "DoubleFirePU";

        // Act
        playerPowerUps.OnTriggerEnter2D(doubleFirePowerUp.GetComponent<Collider2D>());
        yield return new WaitForSeconds(playerPowerUps.doubleFireDuration + 1f);

        // Assert
        Assert.IsFalse(playerPowerUps.IsDoubleFireActive());
    }

    [UnityTest]
    public IEnumerator ShieldPowerUp_DeactivatesAfterDuration()
    {
        // Arrange
        GameObject player = new GameObject();
        PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
        GameObject shieldPowerUp = new GameObject();
        shieldPowerUp.tag = "ShieldPU";

        // Act
        playerPowerUps.OnTriggerEnter2D(shieldPowerUp.GetComponent<Collider2D>());
        yield return new WaitForSeconds(playerPowerUps.shieldDuration + 1f);

        // Assert
        Assert.IsFalse(playerPowerUps.IsShieldActive());
    }

    [UnityTest]
    public IEnumerator ShootingBehavior_WithDoubleFirePowerUp()
    {
        // Arrange
        GameObject player = new GameObject();
        PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
        GameObject doubleFirePowerUp = new GameObject();
        doubleFirePowerUp.tag = "DoubleFirePU";

        playerPowerUps.OnTriggerEnter2D(doubleFirePowerUp.GetComponent<Collider2D>());

        // Act
        playerPowerUps.PerformShoot();

        // Assert
        Assert.AreEqual(2, GameObject.FindObjectsOfType<Projectile>().Length);
    }

    [UnityTest]
    public void ShootingBehavior_WithoutDoubleFirePowerUp()
    {
        // Arrange
        GameObject player = new GameObject();
        PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();

        // Act
        playerPowerUps.PerformShoot();

        // Assert
        Assert.AreEqual(1, GameObject.FindObjectsOfType<Projectile>().Length);
    }
}
