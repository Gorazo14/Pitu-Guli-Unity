using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; private set; }
    public event EventHandler<OnWaveTimerChangedEventArgs> OnWaveTimerChanged;
    public event EventHandler OnNewWave;
    public class OnWaveTimerChangedEventArgs : EventArgs
    {
        public float waveTimerNormalized;
    }
    private enum State
    {
        Idle,
        Active,
        WaitingForNextWave,
    }

    [SerializeField] private EnemyPrefabTypesSO enemyPrefabTypesSO;

    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Transform medkitPrefab;
    [SerializeField] private Transform playerTransform;

    private State state;

    private float waveTimer;
    private int enemyCount = 0;
    private float waveTimerMax = 10f;

    [Header("Wave Settings")]
    [SerializeField] private int enemiesToAdd;
    [SerializeField] private float timeBetweenWavesIncrement;

    private int waveCount = 1;
    private int killCount = 0;

    [SerializeField] private Terrain terrain;
    private void Awake()
    {
        Instance = this;

        enemies = new List<GameObject>();
        waveTimer = waveTimerMax;
        playerTransform.position = GetRandomPositionOnTerrain();
    }
    private void Start()
    {
        Target.OnAnyEnemyDeath += Target_OnAnyEnemyDeath;
        state = State.Idle;
        enemyCount = enemiesToAdd;
        waveTimerMax = 0;
        waveTimer = 0;
    }

    private void Target_OnAnyEnemyDeath(object sender, EventArgs e)
    {
        killCount++;
        enemies.RemoveAt(0);
        if (enemies.Count <= 0)
        {
            state = State.WaitingForNextWave;
        }
    }
    private void Update()
    {
        switch (state)
        {
            default:
            case State.Idle:
                SpawnEnemies();
                state = State.Active;
                break;

            case State.WaitingForNextWave:
                waveTimer += Time.deltaTime;
                OnWaveTimerChanged?.Invoke(this, new OnWaveTimerChangedEventArgs
                {
                    waveTimerNormalized = waveTimer / waveTimerMax
                });
                if (waveTimer >= waveTimerMax)
                {
                    waveTimer = 0f;
                    OnWaveTimerChanged?.Invoke(this, new OnWaveTimerChangedEventArgs
                    {
                        waveTimerNormalized = 0f
                    });
                    state = State.Idle;
                }
                break;
        }
    }
    private void SpawnEnemies()
    {
        if (state == State.Idle)
        {
            GameObject[] prefabs = enemyPrefabTypesSO.prefabs;

            for (int i = 0; i < enemyCount; i++)
            {
                enemies.Add(prefabs[UnityEngine.Random.Range(0, prefabs.Length - 1)]);
                Vector3 randomPosition = GetRandomPositionOnTerrain();
                Instantiate(enemies[i], randomPosition, Quaternion.identity);
            }
            Vector3 randomPositionMedkit = GetRandomPositionOnTerrainForMedkit();
            Instantiate(medkitPrefab, randomPositionMedkit, Quaternion.identity);
            OnNewWave?.Invoke(this, EventArgs.Empty);

            waveCount++;
            enemyCount += enemiesToAdd;
            waveTimerMax += timeBetweenWavesIncrement;
        }
    }
    private Vector3 GetRandomPositionOnTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        float randomX = UnityEngine.Random.Range(130f, 300f);
        float randomZ = UnityEngine.Random.Range(200f, 350f);

        float y = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ)) + terrainPos.y;

        return new Vector3(randomX + terrainPos.x, y + 1f, randomZ + terrainPos.z);
    }
    private Vector3 GetRandomPositionOnTerrainForMedkit()
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;
        float terrainWidth = terrainData.size.x;
        float terrainLength = terrainData.size.z;

        float randomX = UnityEngine.Random.Range(0f, terrainWidth);
        float randomZ = UnityEngine.Random.Range(0f, terrainLength);

        float y = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ)) + terrainPos.y;

        return new Vector3(randomX + terrainPos.x, y, randomZ + terrainPos.z);
    }
    public int GetWaveCount()
    {
        return waveCount;
    }
    public int GetKillCount()
    {
        return killCount;
    }
}
