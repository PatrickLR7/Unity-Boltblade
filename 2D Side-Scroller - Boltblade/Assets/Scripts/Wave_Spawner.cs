using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class Wave_Spawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING }
    public enum LastEnemy { NONE, EYE, BAT, SKELETON }

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

    public Transform eye, bat, skeleton;
    private Wave[] waves;
    private string[] msg;
    public static int currentLevel;
    public static int nextWave;
    private int totalWaves;

    public Transform[] spawnPoints;

    public float timeBetweenWaves;
    private float waveCountdown;

    public SpawnState state = SpawnState.COUNTING;
    private LastEnemy lastSpawned = LastEnemy.NONE;

    private float searchCountdown = 1f;

    public AudioClip levelUp, victory;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        nextWave = 0;
        Debug.Log("Level is: " + currentLevel);

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }

        if(currentLevel == 1)
        {
            timeBetweenWaves = 5f;
            totalWaves = 5;
        }
        else //Level 2
        {
            timeBetweenWaves = 4f;
            totalWaves = 6;
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
                case LastEnemy.BAT:
                    (lastSpawned, m) = DecideFromBat();
                    break;
                case LastEnemy.SKELETON:
                    (lastSpawned, m) = DecideFromSkeleton();
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
                return (LastEnemy.BAT, "50% for bat first");
            case 8:
            case 9:
                return (LastEnemy.SKELETON, "20% for skeleton first");
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
                return (LastEnemy.BAT, "30% from eye to bat");
            case 7:
            case 8:
            case 9:
                return (LastEnemy.SKELETON, "30% from eye to skeleton");
            default:
                return (LastEnemy.NONE, "0%");
        }
    }

    (LastEnemy, string) DecideFromBat()
    {
        int randomNumber = Random.Range(0, 10);
        switch (randomNumber)
        {
            case 0:
            case 1:
            case 2:
                return (LastEnemy.EYE, "30% from bat to eye");
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                return (LastEnemy.BAT, "50% to bat again");
            case 8:
            case 9:
                return (LastEnemy.SKELETON, "20% from bat to skeleton");
            default:
                return (LastEnemy.NONE, "0%");
        }
    }

    (LastEnemy, string) DecideFromSkeleton()
    {
        int randomNumber = Random.Range(0, 10);
        switch (randomNumber)
        {
            case 0:
            case 1:
            case 2:
                return (LastEnemy.EYE, "30% from skeleton to eye");
            case 3:
            case 4:
            case 5:
                return (LastEnemy.BAT, "30% from skeleton to bat");
            case 6:
            case 7:
            case 8:
            case 9:
                return (LastEnemy.SKELETON, "40% to skeleton again");
            default:
                return (LastEnemy.NONE, "0%");
        }
    }

    void AddWave(int position, LastEnemy lastEnemy)
    {
        Wave wave1 = new Wave("Eye Sight", eye, 3, 0.8f);
        Wave wave2 = new Wave("Blood Hunt", bat, 5, 0.7f);
        Wave wave3 = new Wave("Bad to the bone", skeleton, 4, 0.9f);
        switch (lastEnemy)
        {
            case LastEnemy.EYE:
                waves[position] = wave1;
                break;
            case LastEnemy.BAT:
                waves[position] = wave2;
                break;
            case LastEnemy.SKELETON:
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
                audioSource.PlayOneShot(victory, 0.55F);
                nextWave++;
                Debug.Log("All waves completed");
                NextLevel();
            }
            else
            {
                audioSource.PlayOneShot(levelUp, 0.55F);
                nextWave++;
            }
        }

        void NextLevel()
        {
            float delay = 3;
            if(currentLevel == 1)
            {
                currentLevel = 2;
                StartCoroutine(LoadAfterDelay(delay, 0));
            }
            else
            {
                StartCoroutine(LoadAfterDelay(delay, 4));
            }
        }

        IEnumerator LoadAfterDelay(float delay, int newLevel)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(newLevel);
        }

        bool EnemyIsAlive()
        {
            searchCountdown -= Time.deltaTime;
            if (searchCountdown <= 0f)
            {
                searchCountdown = 1f;
                if (GameObject.FindGameObjectWithTag("Bat") == null &&
                    GameObject.FindGameObjectWithTag("Eye") == null &&
                    GameObject.FindGameObjectWithTag("Skeleton") == null)
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
