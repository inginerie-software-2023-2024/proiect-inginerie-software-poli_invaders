using Riptide;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerPowerUps : MonoBehaviour
{
    private bool doubleFire;
    private bool shieldActive;
    private SpriteRenderer playerSprite;
    private Multi_Player player;

    private float doubleFireTime;
    private float shieldTime;

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
        if (doubleFire)
        {
            doubleFireTime -= Time.deltaTime;

            if (doubleFireTime <= 0.0f)
            {
                DeactivateDoubleFirePowerUp();
            }
        }

        if (shieldActive)
        {
            shieldTime -= Time.deltaTime;

            if (shieldTime <= 0.0f)
            {
                DeactivateShieldPowerUp();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (player.IsMain)
            {
                player.SendAction(PlayerActions.shot);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player.IsMain && other.gameObject.TryGetComponent(out MultiPowerUp powerUp))
        {
            Message gotPowerUp = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.playerAction);
            gotPowerUp.AddUShort((ushort)PlayerActions.gotPowerUp);
            gotPowerUp.AddString(powerUp.guid.ToString());

            NetworkManager.Singleton.Client.Send(gotPowerUp);
        }
    }

    public void ActivateDoubleFirePowerUp()
    {
        doubleFire = true;
        doubleFireTime = doubleFireDuration;
    }

    public void DeactivateDoubleFirePowerUp()
    {
        doubleFire = false;
    }

    public void ActivateShieldPowerUp()
    {
        if (!shieldActive)
        {
            shieldActive = true;
            playerSprite.sprite = shieldSprite;

            if (player.IsMain)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
            }
        }

        shieldTime = shieldDuration;
    }

    public void DeactivateShieldPowerUp()
    {
        shieldActive = false;
        playerSprite.sprite = defaultSprite;

        if (player.IsMain)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
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
