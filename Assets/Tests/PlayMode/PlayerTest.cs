using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest
{
    [Test]
    public void Player_TakesDamage_WhenCollidingWithEnemy()
    {
        // Arrange
        GameObject playerObject = new GameObject();
        Player player = playerObject.AddComponent<Player>();
        player.health = 5f;

        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        Collider2D collider = enemyObject.AddComponent<BoxCollider2D>();

        // Act
        player.OnTriggerEnter2D(collider);

        // Assert
        Assert.AreEqual(4f, player.health);
    }

    [Test]
    public void Player_Dies_WhenHealthReachesZero()
    {
        // Arrange
        GameObject playerObject = new GameObject();
        Player player = playerObject.AddComponent<Player>();
        player.health = 1f;

        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        Collider2D collider = enemyObject.AddComponent<BoxCollider2D>();

        // Act
        player.OnTriggerEnter2D(collider);

        // Assert
        Assert.IsFalse(playerObject.activeSelf);
    }

    [Test]
    public void Player_SpriteChangesToHurtSprite_WhenTakingDamage()
    {
        // Arrange
        GameObject playerObject = new GameObject();
        Player player = playerObject.AddComponent<Player>();
        player.hurtSprite = Resources.Load<Sprite>("HurtSprite");

        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        Collider2D collider = enemyObject.AddComponent<BoxCollider2D>();

        // Act
        player.OnTriggerEnter2D(collider);

        // Assert
        Assert.AreEqual(player.hurtSprite, player.playerSr.sprite);
    }

    [Test]
    public void Player_SpriteChangesToDefaultSprite_AfterHurtDuration()
    {
        // Arrange
        GameObject playerObject = new GameObject();
        Player player = playerObject.AddComponent<Player>();
        player.defaultSprite = Resources.Load<Sprite>("DefaultSprite");
        player.hurtSprite = Resources.Load<Sprite>("HurtSprite");
        player.hurtDuration = 0.5f;

        GameObject enemyObject = new GameObject();
        Enemy enemy = enemyObject.AddComponent<Enemy>();

        Collider2D collider = enemyObject.AddComponent<BoxCollider2D>();

        // Act
        player.OnTriggerEnter2D(collider);
        player.Update(); // Simulate passing time

        // Assert
        Assert.AreEqual(player.defaultSprite, player.playerSr.sprite);
    }

}