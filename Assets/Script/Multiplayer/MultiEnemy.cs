using Riptide;
using System;
using UnityEngine;

public enum PowerUpType : ushort
{
    none = 0,
    doubleFire = 1,
    shield = 2,
}

public class MultiEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public Guid guid;

    [SerializeField] private Vector3 direction;
    [SerializeField] private DoubleFire _doubleFirePU;
    [SerializeField] private ShieldPU _shieldPU;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.0f;
    }

    public void SetSpeed(float speed)
    {
        rb.velocity = direction * speed;
    }

    public void SetGuid(Guid guid)
    {
        this.guid = guid;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.TryGetComponent<MultiProjectile>(out var projectile))
        {
            if (projectile.player.IsMain)
            {
                Message enemyHurt = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.enemyHurt);
                enemyHurt.AddString(guid.ToString());

                NetworkManager.Singleton.Client.Send(enemyHurt);
            }
        }
    }
}
