using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyRB;
    public float interval;
    public static float enemySpeed = 5f;
    public float speedUpdateInterval = 5f;
    public float speedIncreaseAmount = 2f;

    private void Start()
    {
        StartCoroutine(InstantiateEnemyCoroutine());
        StartCoroutine(UpdateSpeedCoroutine());
    }

    // Instantiate an enemy every interval seconds
    IEnumerator InstantiateEnemyCoroutine()
    {
        while (true)
        {
            InstantiateEnemy();
            yield return new WaitForSeconds(interval);
        }
    }

    // Update the speed of all enemies every speedUpdateInterval seconds
    IEnumerator UpdateSpeedCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(speedUpdateInterval);
            UpdateEnemySpeed();
        }
    }

    // Instantiate an enemy at a random position on the y-axis with a speed and direction
    void InstantiateEnemy()
    {
        if (Player.instance.health == 0)
        {
            return;
        }

        Vector3 randomPosition = new Vector3(this.transform.position.x, Random.Range(-9f, 6.5f), 0);
        GameObject enemyObj = Instantiate(enemyRB, randomPosition, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.direction = Vector3.left; // Set the desired direction
        enemy.SetSpeed(enemySpeed); // Set the initial speed
    }

    // Update the speed of all enemies
    void UpdateEnemySpeed()
    {
        enemySpeed += speedIncreaseAmount;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.SetSpeed(enemySpeed);
        }
    }

    // Stop all coroutines when the player dies
    void Update()
    {
        if (Player.instance != null && Player.instance.health == 0)
        {
            enemySpeed = 5f;
            StopAllCoroutines();
        }
    }
}
