using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.VFX;

public class WaveController : MonoBehaviour
{

    public UnityEvent OnWin;
    public UnityEvent OnLoose;
    public UnityEvent OnReset;

    public bool startWaveSpawning;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private Transform leftSpawner;
    [SerializeField] private Transform rightSpawner;
    [SerializeField] private Post lastPost;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text enemyDisplay;
    [SerializeField] private VisualEffect gameOverVFX;
    [SerializeField] private int EnemiesAtLastPostToDie = 10;
    [SerializeField] private Valve.VR.InteractionSystem.LinearMapping DeathProgress;

    [SerializeField] private List<Wave> waves;
    [SerializeField] GameOverVFX GameOverVFX;

    private Dictionary<Wave, float> waveTimeBetweenSpawns = new Dictionary<Wave, float>();
    private Dictionary<Wave, int> waveSpawnedEnemies = new Dictionary<Wave, int>();

    private List<GameObject> enemies = new List<GameObject>();

    private int waveCounter = 0;
    private float totalTime = 0;
    private float relativeTime = 0;
    private float spawnFrequency = 0;
    private float enemyCounter = 0;

    private int spawnedNumberOfEnemies = 0;
    private int totalNumberOfEnemies = 0;
    private int totalNumberOfDeademies = 0;
    private bool won = false;
    
    private int EnemiesAtLastPost = 0;

    private void Start()
    {
        lastPost.OnPostWasReached.AddListener(CountEnemies);

        foreach(Wave wave in waves)
        {
            if (wave.SpawnFromBothSpawners)
            {
                totalNumberOfEnemies += wave.NumberOfEnemies * 2;
            }
            else
            {
                totalNumberOfEnemies += wave.NumberOfEnemies;
            }
        }
    }

    private void OnDestroy()
    {
        lastPost.OnPostWasReached.RemoveListener(CountEnemies);
    }

    public void StartWaveSpawning()
    {
        startWaveSpawning = true;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if(enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }
        enemies.Clear();

        OnReset.Invoke();
        UpdateDisplay();

        EnemiesAtLastPost = -1;
        CountEnemies();
    }

    private void CountEnemies()
    {
        EnemiesAtLastPost += 1;
        DeathProgress.value = (float)EnemiesAtLastPost/(float)EnemiesAtLastPostToDie;
        if( EnemiesAtLastPost >= EnemiesAtLastPostToDie)
            StopWaveSpawning();
    }

    public void StopWaveSpawning()
    {
        startWaveSpawning = false;

        // Reset variables 
        waveCounter = 0;
        totalTime = 0;
        relativeTime = 0;
        spawnFrequency = 0;
        enemyCounter = 0;

        spawnedNumberOfEnemies = 0;
        won = false;

        waveTimeBetweenSpawns.Clear();
        waveSpawnedEnemies.Clear();

        // Stop Enemies
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().StopMovement();
        }

        OnLoose.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
        if (!startWaveSpawning)
        {
            return;
        }

        totalTime += Time.deltaTime;

        bool waveActive = false;
        Vector3 pos;

        if(spawnedNumberOfEnemies >= totalNumberOfEnemies && !won)
        {
            Debug.Log("We spawned all Enemies");
            if (totalNumberOfDeademies >= totalNumberOfEnemies)
            {
                Debug.Log("We won");
                OnWin.Invoke();
                won = true;
            }
        }

        foreach(Wave wave in waves)
        {
            if(wave.RelativeToLastWave)
            {
                continue;
            }

            if(!waveTimeBetweenSpawns.ContainsKey(wave))
            {
                waveTimeBetweenSpawns.Add(wave, 0);
            }

            if(!waveSpawnedEnemies.ContainsKey(wave))
            {
                waveSpawnedEnemies.Add(wave, 0);
            }

            if(wave.TimeTillSpawn > totalTime)
            {
                continue;
            }

            waveActive = true;

            float timeBetweenSpawns;
            waveTimeBetweenSpawns.TryGetValue(wave, out timeBetweenSpawns);
            timeBetweenSpawns += Time.deltaTime;
            waveTimeBetweenSpawns[wave] = timeBetweenSpawns;

            if(timeBetweenSpawns < wave.TimeBetweenSpawns)
            {
                continue;
            }

            int spawnedEnemies;
            waveSpawnedEnemies.TryGetValue(wave, out spawnedEnemies);

            if(wave.NumberOfEnemies <= spawnedEnemies)
            {
                waveActive = false;
                continue;
            }

            waveTimeBetweenSpawns[wave] = 0;
            waveSpawnedEnemies[wave] = ++spawnedEnemies;

            if(!wave.SpawnFromBothSpawners)
            {
                pos = (wave.Spawner == 0) ? leftSpawner.position : rightSpawner.position;
            } 
            else
            {
                SpawnEnemy(rightSpawner.position);
                pos = leftSpawner.position;
            }

            SpawnEnemy(pos);
            UpdateDisplay();
        }


        if(waveCounter > waves.Count - 1)
        {
            return;
        }

        if (!waves[waveCounter].RelativeToLastWave)
        {
            waveCounter++;
            return;
        }

        // This exists to prevent an already started wave from getting interupted
        if(waveActive && relativeTime < waves[waveCounter].TimeTillSpawn)
        {
            relativeTime = 0;
            return;
        }

        relativeTime += Time.deltaTime;

        if(relativeTime < waves[waveCounter].TimeTillSpawn)
        {
            return;
        }

        spawnFrequency += Time.deltaTime;

        if(spawnFrequency < waves[waveCounter].TimeBetweenSpawns)
        {
            return;
        }

        if(enemyCounter >= waves[waveCounter].NumberOfEnemies)
        {
            relativeTime = 0;
            enemyCounter = 0;
            waveCounter++;
            return;
        }

        spawnFrequency = 0;
        enemyCounter++;
        
        if (!waves[waveCounter].SpawnFromBothSpawners)
        {
            pos = (waves[waveCounter].Spawner == 0) ? leftSpawner.position : rightSpawner.position;
        }
        else
        {
            SpawnEnemy(rightSpawner.position);
            pos = leftSpawner.position;
        }

        SpawnEnemy(pos);
        UpdateDisplay();
    }

    private void SpawnEnemy(Vector3 position)
    {
        GameObject enemy = Instantiate(Enemy, position, new Quaternion());
        enemy.GetComponent<Enemy>().wavecontroller = this;
        enemies.Add(enemy);
        spawnedNumberOfEnemies++;
    }

    public void EnemyKilled()
    {
        totalNumberOfDeademies += 1;
    }

    private void UpdateDisplay()
    {
        enemyDisplay.text = spawnedNumberOfEnemies + " / " + totalNumberOfEnemies + " Enemies";
    }
}
