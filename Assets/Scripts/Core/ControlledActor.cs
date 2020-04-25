using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public class ControlledActor : MonoBehaviour, IControlledActor
{
    private IPlayer owner;
    public IPlayer Owner => owner;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private Transform forwardPoint;

    [SerializeField]
    private SpriteRenderer _spriteRend;

    [SerializeField]
    private PlayerStatus _messageText;

    [SerializeField]
    private CircleCollider2D _collider;

    [SerializeField]
    private Transform _gunTransform;

    [SerializeField]
    private Projectile _projectilePrefab;

    private int _playerIndex = -1;

    private int MaxHealth => GameController.Instance.GameConfig.MaxHealth;
    private int health;

    private int _currentScore = 0;
    public int Score => _currentScore;

    public void SetActorData(IPlayer owner, int playerIndex)
    {
        this.owner = owner;
        _playerIndex = playerIndex;
        _spriteRend.color = GetColor();
        health = MaxHealth;
    }

    public Color GetColor()
    {
        return GameController.Instance.GameConfig.GetColorByIndex(_playerIndex);
    }

    public Vector2 GetPosition()
    {
        return transform.position.ToVector2();
    }

    public Vector2 GetVelocity()
    {
        return _rb.velocity;
    }

    public Vector2 GetForwardVector()
    {
        return (forwardPoint.position - transform.position).ToVector2().normalized;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }
    
    public void Message(string text)
    {
        _messageText.ShowMessage(text, 3f);
        //this.text.text = text;
    }
    [SerializeField]
    private PlayerStatus _messageTextPrefab;

    private void Start()
    {
        _messageText = Instantiate(_messageTextPrefab);
        _messageText.Follow(this.transform);
    }

    public void Shoot()
    {
        if (MayShoot())
        {
            var spawnedProjectile = Instantiate(_projectilePrefab, _gunTransform.position, Quaternion.identity);
            spawnedProjectile.Initialize(this);
            _lastShootTime = Time.time;
        }
    }
    private float _lastShootTime = -1000f;
    public float TimeToNextShoot => Mathf.Clamp((GameController.Instance.GameConfig.ShootingReloadTime - (Time.time - _lastShootTime)), 0f, GameController.Instance.GameConfig.ShootingReloadTime);

    private bool MayShoot()
    {
        return TimeToNextShoot <= 0;
    }

    public void GetDamage(int damage)
    {
        health = health - damage;
        if(health <= 0)
        {
            GameController.Instance.KillActor(this);
        }
        _messageText.UpdateHealth(health, MaxHealth);
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void Heal(int heal)
    {
        health = Mathf.Clamp(health + heal, 0, MaxHealth);
        _messageText.UpdateHealth(health, MaxHealth);
    }

    public float AngleTo(Vector2 target)
    {
        var currentForward = GetForwardVector().normalized;
        var targetForward = (target - transform.position.ToVector2()).normalized;
        return Vector2.SignedAngle(currentForward, targetForward);
    }

    public float UnsignedAngleTo(Vector2 target)
    {
        var currentForward = GetForwardVector().normalized;
        var targetForward = (target - transform.position.ToVector2()).normalized;
        return Vector2.Angle(currentForward, targetForward);
    }

    public void RotateTo(Vector2 target)
    {
        var currentForward = GetForwardVector().normalized;
        var targetForward = (target - transform.position.ToVector2()).normalized;
        var angle = Vector2.SignedAngle(currentForward, targetForward);
        //Debug.Log("C:" + currentForward + "|T:" + targetForward + "|A:" + angle);
        actionsCache.rotation = angle;
    }

    public void LookAt(Vector2 target)
    {
        RotateTo(target);
    }

    public void RotateLeft(float angle)
    {
        actionsCache.rotateLeft = angle;
    }

    public void RotateRight(float angle)
    {
        actionsCache.rotateRight = angle;
    }

    public void MoveForward(float force)
    {
        actionsCache.move += GetForwardVector() * force;
    }

    public void MoveBackward(float force)
    {
        actionsCache.move += -GetForwardVector() * force;
    }

    public void MoveLeft(float force)
    {        
        actionsCache.move += Vector2.Perpendicular(GetForwardVector()) * force;
    }

    public void MoveRight(float force)
    {
        actionsCache.move += -Vector2.Perpendicular(GetForwardVector()) * force;
    }
    public void MoveTo(Vector2 target, float force)
    {
        actionsCache.move += (target - transform.position.ToVector2()).normalized * force;
    }

    public ActorInfo Info
    {
        get
        {
            return new ActorInfo(position: transform.position.ToVector2()
                , velocity: _rb.velocity
                , forwardVector: GetForwardVector()
                , rotation: transform.rotation
                , name: owner.Name
                , radius: _collider.radius
                , currentScore: _currentScore
                , timeToNextShoot: TimeToNextShoot
                , health: health
                , maxHealth: MaxHealth);
        }
    }

    public void AddScore(int score)
    {
        _currentScore += score;
        GameController.Instance.Leaderboard.UpdateLeaderboard();
    }
    private ActionsCache actionsCache = new ActionsCache();

    public void DoCachedActions()
    {
        var maxVelocity = GameController.Instance.GameConfig.MaxVelocity;
        var maxRotation = GameController.Instance.GameConfig.MaxRotationAngle;
        if (_rb.velocity.magnitude < maxVelocity)
        {
            if(actionsCache.move.magnitude > maxVelocity)
            {
                actionsCache.move = actionsCache.move.normalized * maxVelocity;
            }
            _rb.AddForce(actionsCache.move);
        }

        var rot = transform.rotation;
        var zRot = Mathf.Clamp(actionsCache.rotation + actionsCache.rotateLeft - actionsCache.rotateRight, -maxRotation, maxRotation);
        rot = rot * Quaternion.Euler(0, 0, zRot);
        _rb.MoveRotation(rot);
        if (actionsCache.shoot)
        {
            Shoot();
        }
        actionsCache.Clear();
    }

    public float MaxVelocity
    {
        get
        {
            return GameController.Instance.GameConfig.MaxVelocity;
        }
    }

    public float MaxRotationAngle
    {
        get
        {
            return GameController.Instance.GameConfig.MaxRotationAngle;
        }
    }
}

public class ActionsCache
{
    public float rotateLeft = 0;
    public float rotateRight = 0;
    public float rotation = 0;
    public Vector2 move = Vector2.zero;
    public bool shoot = false;

    public void Clear()
    {
        rotateLeft = 0;
        rotateRight = 0;
        rotation = 0;
        move = Vector2.zero;
        shoot = false;
    }
}
