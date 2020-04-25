using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;
using System.Reflection;

public class GameContext : object, IGameContext
{
    public float ElapsedTime { get; set; }

    public float RemainTime { get; set; }

    public int FragScore { get
        {
            return GameController.Instance.GameConfig.FragScore;
        }
    }

    private Borders _borders;
    public Borders WorldBorders => _borders;

    private List<BonusInfo> _bonuses = new List<BonusInfo>();
    private List<ActorInfo> _enemies = new List<ActorInfo>();
    private List<ProjectileInfo> _projectiles = new List<ProjectileInfo>();
       
    public void SetBorders(float left, float right, float up, float down)
    {
        _borders = new Borders(left, right, up, down);
    }

    public void SetEnemies(List<ActorInfo> enemies)
    {
        _enemies.AddRange(enemies);
    }

    public void SetProjectiles(List<ProjectileInfo> projectiles)
    {
        _projectiles.AddRange(projectiles);
    }

    public void SetBonuses(List<BonusInfo> bonuses)
    {
        _bonuses.AddRange(bonuses);
    }

    public List<BonusInfo> GetBonusInfo()
    {
        return _bonuses;
    }

    public List<ActorInfo> GetEnemiesInfo()
    {
        return _enemies;
    }

    public List<ProjectileInfo> GetProjectilesInfo()
    {
        return _projectiles;
    }
}
