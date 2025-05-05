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

    private State state;

    private float waveTimer;
    private int enemyCount = 0;
    private float waveTimerMax = 10f;

    [Header("Wave Settings")]
    [SerializeField] private int enemiesToAdd;
    [SerializeField] private float timeBetweenWavesIncrement;

    private int waveCount = 1;

    private void Awake()
    {
        Instance = this;

        enemies = new List<GameObject>();
        waveTimer = waveTimerMax;
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
                    state = State.Idle;
                }
                break;
        }
        Debug.Log(enemyCount);
    }
    private void SpawnEnemies()
    {
        if (state == State.Idle)
        {
            GameObject[] prefabs = enemyPrefabTypesSO.prefabs;

            for (int i = 0; i < enemyCount; i++)
            {
                enemies.Add(prefabs[UnityEngine.Random.Range(0, prefabs.Length - 1)]);
                Instantiate(enemies[i], new Vector3(UnityEngine.Random.Range(27, 33), 0, UnityEngine.Random.Range(27, 33)), Quaternion.identity);
            }

            OnNewWave?.Invoke(this, EventArgs.Empty);

            waveCount++;
            enemyCount += enemiesToAdd;
            waveTimerMax += timeBetweenWavesIncrement;
        }
    }
    public int GetWaveCount()
    {
        return waveCount;
    }
}
