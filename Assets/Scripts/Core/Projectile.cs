using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private CircleCollider2D _collider;

    [SerializeField]
    private SpriteRenderer _spriteRend;

    public int Damage => GameController.Instance.GameConfig.ProjectileDamage;

    private float _spawnTime;
    private float RemainLifeTime
    {
        get
        {
             return GameController.Instance.GameConfig.ProjectileLifeTime - (Time.time - _spawnTime);
        }
    }

    private void Update()
    {
        if(Time.time - _spawnTime >= GameController.Instance.GameConfig.ProjectileLifeTime)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        GameController.Instance.UnregisterProjectile(this);
    }

    public ProjectileInfo GetInfo()
    {
        var info = new ProjectileInfo(radius: _collider.radius * transform.localScale.x, 
            position:transform.position.ToVector2(), 
            velocity:_rb.velocity,
            owner: _owner.name,
            remainLifeTime : RemainLifeTime,
            damage: Damage);
        return info;
    }
    private ControlledActor _owner;

    public void Initialize(ControlledActor owner)
    {
        _spawnTime = Time.time;
        _owner = owner;
        _spriteRend.color = owner.GetColor();
        _rb.velocity = owner.GetForwardVector().normalized * GameController.Instance.GameConfig.ProjectileVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var actor = collision.gameObject.GetComponent<ControlledActor>();
        if (actor != null)
        {
            if (actor != _owner)
            {
                actor.GetDamage(Damage);
                _owner.AddScore(Damage);
                if(actor.IsDead())
                {
                    _owner.AddScore(GameController.Instance.GameConfig.FragScore);
                }
                Destroy(this.gameObject);
            }
        }
        if(collision.gameObject.name.Contains("Border"))
        {
            Destroy(this.gameObject);
        }
    }
}
