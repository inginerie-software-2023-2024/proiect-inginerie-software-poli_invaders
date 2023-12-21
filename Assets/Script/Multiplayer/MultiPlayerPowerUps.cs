using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerPowerUps : MonoBehaviour
{
    private bool doubleFire;
    private bool shieldActive;
    private SpriteRenderer playerSprite;
    private Multi_Player player;

    [SerializeField] private MultiProjectile laserPrefab;
    [SerializeField] private Sprite shieldSprite;
    [SerializeField] private Sprite defaultSprite;

    [SerializeField] private float doubleFireDuration;
    [SerializeField] private float shieldDuration;

    private void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        player = GetComponent<Multi_Player>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        doubleFire = false;
        shieldActive = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (player.IsMain)
            {
                player.SendAction(PlayerActions.shot);
            }
        }
    }

    public void Shoot()
    {
        List<Vector3> laserOffsets = new();
        
        if (doubleFire)
        {
            laserOffsets.Add(new Vector3(0.99f, 0.43f, 0.0f));
            laserOffsets.Add(new Vector3(0.99f, 0.27f, 0.0f));
        }
        else
        {
            laserOffsets.Add(new Vector3(0.99f, 0.36f, 0.0f));
        }

        foreach (Vector3 offset in laserOffsets)
        {
            Vector3 spawnPosition = transform.position + offset;
            MultiProjectile laser = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);
            laser.SetPlayer(player);
        }
    }
}
