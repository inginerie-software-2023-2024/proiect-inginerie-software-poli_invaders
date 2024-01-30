using Riptide;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiEnemySpawn : MonoBehaviour
{
    private static MultiEnemySpawn _singleton;

    public static MultiEnemySpawn Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else
            {
                Debug.Log($"{nameof(MultiEnemySpawn)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private List<GameObject> enemyTypes;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float speedIncreaseAmount;

    private Dictionary<Guid, MultiEnemy> enemyList;

    private void Awake()
    {
        Singleton = this;
        enemyList = new Dictionary<Guid, MultiEnemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MessageHandler((ushort)ServerToClientId.newEnemy)]
    private static void SpawnNewEnemy(Message message)
    {
        Guid guid = new(message.GetString());
        ushort enemyType = message.GetUShort();
        Vector3 enemyPosition = message.GetVector3();

        GameObject enemyObj = Instantiate(Singleton.enemyTypes[enemyType], enemyPosition, Quaternion.identity);
        MultiEnemy enemy = enemyObj.GetComponent<MultiEnemy>();
        enemy.SetDirection(Vector3.left); // Set the desired direction
        enemy.SetSpeed(Singleton.enemySpeed); // Set the initial speed
        enemy.SetGuid(guid);

        Singleton.enemyList.Add(guid, enemy);
    }

    [MessageHandler((ushort)ServerToClientId.updateEnemySpeed)]
    private static void UpdateEnemySpeed(Message _)
    {
        Singleton.enemySpeed += Singleton.speedIncreaseAmount;

        foreach ((Guid _, MultiEnemy enemy) in Singleton.enemyList)
        {
            enemy.SetSpeed(Singleton.enemySpeed);
        }
    }

    [MessageHandler((ushort)ServerToClientId.enemyDeath)]
    private static void EnemyDeath(Message message)
    {
        Guid guid = new(message.GetString());
        if (Singleton.enemyList.ContainsKey(guid))
        {
            MultiEnemy enemy = Singleton.enemyList[guid];

            ushort powerUpType = message.GetUShort();
            if ((PowerUpType)powerUpType != PowerUpType.none)
            {
                MultiPowerUpManager.Singleton.SpawnNewPowerUp(new(message.GetString()), powerUpType, enemy.transform.position);
            }

            Destroy(enemy.gameObject);
            Singleton.enemyList.Remove(guid);
            Score.Instance.AddScore(1);
        }
    }

    public int EnemyCount()
    {
        return enemyList.Count;
    }
}
