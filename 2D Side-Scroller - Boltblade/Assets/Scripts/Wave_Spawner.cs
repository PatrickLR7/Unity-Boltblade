using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave_Spawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING }
    public enum LastEnemy { NONE, EYE, GHOST, SLIME }

    [System.Serializable]
    public class Wave
    {
        public Wave(string name, Transform enemy, int count, float rate)
        {
            this.name = name;
            this.enemy = enemy;
            this.count = count;
            this.rate = rate;
        }
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Transform eye, ghost, slime;
    private Wave[] waves;
    private string[] msg;
    private int nextWave = 0;
    private const int totalWaves = 5;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    public SpawnState state = SpawnState.COUNTING;
    private LastEnemy lastSpawned = LastEnemy.NONE;

    private float searchCountdown = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }
        waveCountdown = timeBetweenWaves;
        DecideWaves();
    }

    void DecideWaves()
    {
        waves = new Wave[totalWaves];
        msg = new string[totalWaves];

        string m = "";
        for (int i = 0; i < totalWaves; i++)
        {
            switch (lastSpawned)
            {
                case LastEnemy.NONE:
                    (lastSpawned, m) = DecideFromNone();
                    break;
                case LastEnemy.EYE:
                    (lastSpawned, m) = DecideFromEye();
                    break;
                case LastEnemy.GHOST:
                    (lastSpawned, m) = DecideFromGhost();
                    break;
                case LastEnemy.SLIME:
                    (lastSpawned, m) = DecideFromSlime();
                    break;
            }
            AddWave(i, lastSpawned);
            msg[i] = m;
        }
    }

    (LastEnemy, string) DecideFromNone()
    {
        int randomNumber = Random.Range(0, 10);
        switch (randomNumber)
        {
            case 0:
            case 1:
            case 2:
                return (LastEnemy.EYE, "30% for eye first");
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                return (LastEnemy.GHOST, "50% for ghost first");
            case 8:
            case 9:
                return (LastEnemy.SLIME, "20% for slime first");
            default:
                return (LastEnemy.NONE, "0%");
        }
    }

    (LastEnemy, string) DecideFromEye()
    {
        int randomNumber = Random.Range(0, 10);
        switch (randomNumber)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return (LastEnemy.EYE, "40% to eye again");
            case 4:
            case 5:
            case 6:
                return (LastEnemy.GHOST, "30% from eye to ghost");
            case 7:
            case 8:
            case 9:
                return (LastEnemy.SLIME, "30% from eye to slime");
            default:
                return (LastEnemy.NONE, "0%");
        }
    }

    (LastEnemy, string) DecideFromGhost()
    {
        int randomNumber = Random.Range(0, 10);
        switch (randomNumber)
        {
            case 0:
            case 1:
            case 2:
                return (LastEnemy.EYE, "30% from ghost to eye");
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                return (LastEnemy.GHOST, "50% to ghost again");
            case 8:
            case 9:
                return (LastEnemy.SLIME, "20% from ghost to slime");
            default:
                return (LastEnemy.NONE, "0%");
        }
    }

    (LastEnemy, string) DecideFromSlime()
    {
        int randomNumber = Random.Range(0, 10);
        switch (randomNumber)
        {
            case 0:
            case 1:
            case 2:
                return (LastEnemy.EYE, "30% from slime to eye");
            case 3:
            case 4:
            case 5:
                return (LastEnemy.GHOST, "30% from slime to ghost");
            case 6:
            case 7:
            case 8:
            case 9:
                return (LastEnemy.SLIME, "40% to slime again");
            default:
                return (LastEnemy.NONE, "0%");
        }
    }

    void AddWave(int position, LastEnemy lastEnemy)
    {
        Wave wave1 = new Wave("Eye Sight", eye, 4, 3);
        Wave wave2 = new Wave("Haunted Hunt", ghost, 8, 2);
        Wave wave3 = new Wave("Lame Slime", slime, 5, 1.5f);
        switch (lastEnemy)
        {
            case LastEnemy.EYE:
                waves[position] = wave1;
                break;
            case LastEnemy.GHOST:
                waves[position] = wave2;
                break;
            case LastEnemy.SLIME:
                waves[position] = wave3;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING && nextWave < totalWaves)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }

        void WaveCompleted()
        {
            Debug.Log("Wave completed");
            state = SpawnState.COUNTING;
            waveCountdown = timeBetweenWaves;
            if (nextWave + 1 == totalWaves)
            {
                //nextWave = 0;
                nextWave++;
                Debug.Log("All waves completed");
            }
            else
            {
                nextWave++;
            }
        }

        bool EnemyIsAlive()
        {
            searchCountdown -= Time.deltaTime;
            if (searchCountdown <= 0f)
            {
                searchCountdown = 1f;
                if (GameObject.FindGameObjectWithTag("Enemy") == null)
                {
                    return false;
                }
            }
            return true;
        }

        IEnumerator SpawnWave(Wave _wave)
        {
            Debug.Log(msg[nextWave]);
            state = SpawnState.SPAWNING;

            for (int i = 0; i < _wave.count; i++)
            {
                SpawnEnemy(_wave.enemy);
                yield return new WaitForSeconds(1f / _wave.rate);
            }

            state = SpawnState.WAITING;
            yield break;
        }

        void SpawnEnemy(Transform _enemy)
        {
            //Debug.Log("Spawning enemy " + _enemy.name);
            Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //Instantiate(_enemy, transform.position, transform.rotation);
            Instantiate(_enemy, _sp.position, _sp.rotation);
        }
    }
}
