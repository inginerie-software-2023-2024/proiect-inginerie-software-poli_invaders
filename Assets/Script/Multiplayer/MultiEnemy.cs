using Riptide;
using System;
using UnityEngine;

public enum PowerUp : ushort
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

    public void SpawnPowerUp(ushort type)
    {
        switch ((PowerUp)type)
        {
            case PowerUp.none:
                {
                    break;
                }
            case PowerUp.doubleFire:
                {
                    Instantiate(_doubleFirePU, transform.position, Quaternion.identity);
                    break;
                }
            case PowerUp.shield:
                {
                    Instantiate(_shieldPU, transform.position, Quaternion.identity);
                    break;
                }
            default:
                {
                    Debug.Log("(ENEMY): Wrong PowerUp value.");
                    break;
                }
        }
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
