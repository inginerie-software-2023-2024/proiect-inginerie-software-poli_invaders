using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPowerUp : MonoBehaviour
{
    public Guid guid;

    [SerializeField] private PowerUpType type;
    

    public void SetGuid(Guid guid)
    {
        this.guid = guid;
    }

    public PowerUpType GetPowerUpType()
    {
        return type;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
