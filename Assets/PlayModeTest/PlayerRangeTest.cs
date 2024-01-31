using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class PlayerRangeTest
{
    [UnityTest]
    public IEnumerator DoubleFirePowerUp_ActivatesCorrectly()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        PlayerPowerUps playerPowerUps = gameObject.AddComponent<PlayerPowerUps>();
        bool doubleFire = false;

        // Act
        playerPowerUps.ActivateDoubleFirePowerUp();

        // Assert
        yield return null; // Wait for the end of frame
        Assert.IsTrue(playerPowerUps.doubleFire == true);
    }

    [UnityTest]
    public IEnumerator ShieldPowerUp_ActivatesCorrectly()
    {
        // Arrange
        GameObject gameObject = new GameObject();
        PlayerPowerUps playerPowerUps = gameObject.AddComponent<PlayerPowerUps>();
        bool shieldActive = false;

        // Act
        playerPowerUps.ActivateShieldPowerUp();

        // Assert
        yield return null; // Wait for the end of frame
        Assert.IsTrue(playerPowerUps.shieldActive == true);
    }

    // [UnityTest]
    // public IEnumerator DoubleFirePowerUp_DeactivatesAfterDuration()
    // {
    //     // Arrange
    //     GameObject player = new GameObject();
    //     PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
    //     bool doubleFire = false;

    //     // Act
    //     playerPowerUps.ActivateDoubleFirePowerUp();
    //     yield return new WaitForSeconds(playerPowerUps.doubleFireDuration + 1f);

    //     // Assert
    //     Assert.IsFalse(playerPowerUps.doubleFire == false);
    // }

    // [UnityTest]
    // public IEnumerator ShieldPowerUp_DeactivatesAfterDuration()
    // {
    //     // Arrange
    //     GameObject player = new GameObject();
    //     PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
    //     bool shieldActive = false;

    //     // Act
    //     playerPowerUps.ActivateShieldPowerUp();
    //     yield return new WaitForSeconds(playerPowerUps.shieldDuration + 1f);

    //     // Assert
    //     Assert.IsFalse(playerPowerUps.shieldActive == false);
    // }

    // [UnityTest]
    // public IEnumerator ShootingBehavior_WithDoubleFirePowerUp()
    // {
    //     // Arrange
    //     GameObject player = new GameObject();
    //     PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
    //     bool doubleFire = true;

    //     // Act
    //     playerPowerUps.PerformShoot();

    //     // Assert
    //     Assert.AreEqual(2, GameObject.FindObjectsOfType<Projectile>().Length);
    //     yield return null;
    // }

    // [UnityTest]
    // public IEnumerator ShootingBehavior_WithoutDoubleFirePowerUp()
    // {
    //     // Arrange
    //     GameObject player = new GameObject();
    //     PlayerPowerUps playerPowerUps = player.AddComponent<PlayerPowerUps>();
    //     bool doubleFire = false;

    //     // Act
    //     playerPowerUps.PerformShoot();

    //     // Assert
    //     Assert.AreEqual(1, GameObject.FindObjectsOfType<Projectile>().Length);
    //     yield return null;
    // }
}
