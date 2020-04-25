using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayersConfig", menuName = "Players Config")]
public class PlayersConfig : ScriptableObject
{
    [SerializeField]
    private List<Color> _colors;


    [SerializeField]
    private float maxVelocity;
    public float MaxVelocity => maxVelocity;

    [SerializeField]
    private float maxRotationAngle;
    public float MaxRotationAngle => maxRotationAngle;

    [SerializeField]
    private float projectileVelocity;
    public float ProjectileVelocity => projectileVelocity;

    [SerializeField]
    private float projectileLifeTime;
    public float ProjectileLifeTime => projectileLifeTime;

    [SerializeField]
    private float shootingReloadTime;
    public float ShootingReloadTime => shootingReloadTime;

    [SerializeField]
    private int maxHealth;
    public int MaxHealth => maxHealth;

    [SerializeField]
    private float respawnTime;
    public float RespawnTime => respawnTime;

    [SerializeField]
    private int projectileDamage = 10;
    public int ProjectileDamage => projectileDamage;

    [SerializeField]
    private int fragScore = 10;
    public int FragScore => fragScore;

    [SerializeField]
    private List<Bonus> bonusPrefabs;

    [SerializeField]
    private int maxBonuses;
    public int MaxBonuses => maxBonuses;

    [SerializeField]
    private float minBonusSpawnTime;
    public float MinBonusSpawnTime => minBonusSpawnTime;

    [SerializeField]
    private float maxBonusSpawnTime;
    public float MaxBonusSpawnTime => maxBonusSpawnTime;

    [SerializeField]
    private float gameTime;
    public float GameTime => gameTime;

    public Color GetColorByIndex(int index)
    {
        var clr = _colors[index];
        clr.a = 0.6f;
        return clr;
    }

    public Bonus GetRandomBonusPrefab()
    {
        return bonusPrefabs[Random.Range(0, bonusPrefabs.Count)];
    }
}
