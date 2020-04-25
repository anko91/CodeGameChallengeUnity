using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeGameChallengeInterfaces;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private ControlledActor _playerPrefab;

    private List<ControlledActor> playersActors = new List<ControlledActor>();

    public PlayersSelector selector;

    [SerializeField]
    private List<Transform> _spawnPositions;

    [SerializeField]
    private PlayersConfig _playersConfig;
    public PlayersConfig GameConfig => _playersConfig;

    [SerializeField]
    private Leaderboard _leaderboard;
    public Leaderboard Leaderboard => _leaderboard;
    
    private List<Projectile> _projectiles = new List<Projectile>();
    private List<Bonus> _bonuses = new List<Bonus>();

    [SerializeField]
    private GameMode _gameMode = GameMode.DLL;

    public void RegisterProjectile(Projectile proj)
    {
        _projectiles.Add(proj);
    }

    public void UnregisterProjectile(Projectile proj)
    {
        _projectiles.Remove(proj);
    }

    public void RegisterBonus(Bonus bonus)
    {
        _bonuses.Add(bonus);
    }

    public void UnregisterBonus(Bonus bonus)
    {
        _bonuses.Remove(bonus);
    }

    public void KillActor(ControlledActor actor)
    {
        StartCoroutine(KillActorRoutine(actor));
    }

    private IEnumerator SpawnBonusesRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(GameConfig.MinBonusSpawnTime, GameConfig.MaxBonusSpawnTime));
            if(_bonuses.Count < GameConfig.MaxBonuses)
            {
                var bonusPrefab = GameConfig.GetRandomBonusPrefab();
                var bonusPosition = new Vector3(
                    Random.Range(_left + bonusPrefab.Radius, _right - bonusPrefab.Radius),
                    Random.Range(_down + bonusPrefab.Radius, _up - bonusPrefab.Radius),
                    0);
                Instantiate(bonusPrefab, bonusPosition, Quaternion.identity);
            }
        }
    }

    private IEnumerator KillActorRoutine(ControlledActor actor)
    {
        playersActors.Remove(actor);
        actor.gameObject.SetActive(false);
        yield return new WaitForSeconds(GameConfig.RespawnTime);
        actor.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;
        actor.Heal(GameConfig.MaxHealth);
        actor.gameObject.SetActive(true);
        playersActors.Add(actor);
    }

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        selector.LoadPlayers(_gameMode);
        
        InitPlayers();
        StartCoroutine(SpawnBonusesRoutine());
    }

    [SerializeField]
    private UnityEngine.UI.Text gameResultText;
    [SerializeField]
    private UnityEngine.UI.Text gameTimerText;

    public float RemainGameTime => GameConfig.GameTime - Time.time;
    public float ElapsedGameTime => Time.time;

    
    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        var remainTime = GameConfig.GameTime - Time.time;
        if (remainTime < 0) remainTime = 0.000f;
        gameTimerText.text = string.Format("{0,3}", remainTime);
        if(RemainGameTime <= 0)
        {
            gameResultText.text = "WINNER: " + _leaderboard.Winner();
            Time.timeScale = 0f;
        }
    }

    public void InitPlayers()
    {
        int playersAmount = selector.PlayersAmount(_gameMode);
        List<int> spawnIndex = new List<int>();
        for (var i = 0; i < playersAmount; i++)
        {
            spawnIndex.Insert(Random.Range(0, spawnIndex.Count), i);
        }
        for (var i = 0; i < playersAmount; i++)
        {
            var sPos = _spawnPositions[spawnIndex[i]];
            var newActor = Instantiate(_playerPrefab, sPos.position, sPos.rotation);
            newActor.SetActorData(selector.GetOwner(i, _gameMode), i);
            playersActors.Add(newActor);
        }
        _leaderboard.InitLeaderboard(playersActors);
    }

    private void FixedUpdate()
    {
        foreach (var player in playersActors)
        {
            player.Owner.Tick(player, GetContextForPlayer(player));
        }

        foreach (var player in playersActors)
        {
            player.DoCachedActions();
        }
    }

    public GameContext GetContextForPlayer(ControlledActor player)
    {
        var context = new GameContext();
        context.ElapsedTime = ElapsedGameTime;
        context.RemainTime = RemainGameTime;
        var enemies = new List<ActorInfo>();
        var projectiles = new List<ProjectileInfo>();
        var bonuses = new List<BonusInfo>();

        foreach (var actor in playersActors)
        {
            if (actor != player)
            {
                enemies.Add(actor.Info);
            }
        }
        foreach (var proj in _projectiles)
        {
            projectiles.Add(proj.GetInfo());
        }
        foreach (var bonus in _bonuses)
        {
            bonuses.Add(bonus.GetInfo());
        }
        context.SetEnemies(enemies);
        context.SetProjectiles(projectiles);
        context.SetBonuses(bonuses);
        context.SetBorders(_left, _right, _up, _down);
        context.RemainTime = RemainGameTime;
        context.ElapsedTime = ElapsedGameTime;

        return context;
    }
    [SerializeField]
    private float _left;
    [SerializeField]
    private float _right;
    [SerializeField]
    private float _up;
    [SerializeField]
    private float _down;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(new Vector3(_left, _up, 0), new Vector3(_right, _up, 0));
        Gizmos.DrawLine(new Vector3(_left, _down, 0), new Vector3(_right, _down, 0));
        Gizmos.DrawLine(new Vector3(_left, _up, 0), new Vector3(_left, _down, 0));
        Gizmos.DrawLine(new Vector3(_right, _up, 0), new Vector3(_right, _down, 0));

        var offset = 0.1f;
        Gizmos.DrawLine(new Vector3(_left - offset, _up + offset, 0), new Vector3(_right + offset, _up + offset, 0));
        Gizmos.DrawLine(new Vector3(_left - offset, _down - offset, 0), new Vector3(_right + offset, _down - offset, 0));
        Gizmos.DrawLine(new Vector3(_left - offset, _up + offset, 0), new Vector3(_left - offset, _down - offset, 0));
        Gizmos.DrawLine(new Vector3(_right + offset, _up + offset, 0), new Vector3(_right + offset, _down - offset, 0));
    }
}

public enum GameMode
{
    DLL = 1,
    Inner = 2
}