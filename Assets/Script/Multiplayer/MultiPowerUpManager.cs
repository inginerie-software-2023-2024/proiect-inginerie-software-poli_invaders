using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPowerUpManager : MonoBehaviour
{
    private static MultiPowerUpManager _singleton;

    public static MultiPowerUpManager Singleton
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
                Debug.Log($"{nameof(MultiPowerUpManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [SerializeField] private List<GameObject> powerUpPrefabs;

    private Dictionary<Guid, MultiPowerUp> powerUpList;

    private void Awake()
    {
        Singleton = this;
        powerUpList = new Dictionary<Guid, MultiPowerUp>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNewPowerUp(Guid guid, ushort type, Vector3 position)
    {
        if (Instantiate(powerUpPrefabs[type - 1], new Vector3(position.x, position.y, -1.0f), Quaternion.identity).TryGetComponent(out MultiPowerUp powerUp))
        {
            powerUp.SetGuid(guid);

            powerUpList[guid] = powerUp;
        }

    }

    public void RemovePowerUp(Guid guid)
    {
        Destroy(powerUpList[guid].gameObject);
        powerUpList.Remove(guid);
    }

    public void ActivatePowerUp(Guid guid, MultiPlayerPowerUps playerPowerUps)
    {
        switch (powerUpList[guid].GetPowerUpType())
        {
            case PowerUpType.doubleFire:
                {
                    playerPowerUps.ActivateDoubleFirePowerUp();
                    break;
                }
            case PowerUpType.shield:
                {
                    playerPowerUps.ActivateShieldPowerUp();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
