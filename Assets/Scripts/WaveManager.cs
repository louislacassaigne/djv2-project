using System.Collections;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public enum Phase
    {
        Preparation,
        Combat
    }

    [Header("Phase")]
    public Phase currentPhase = Phase.Preparation;

    [Header("Wave")]
    public int waveIndex = 1;
    public float waveDuration = 60f;

    [Header("Combat")]
    public int enemiesRemainingToSpawn = 0;
    public int enemiesAlive = 0;

    [Header("Multiplicateur")]
    public float multiplier = 1f;

    [Header("Spawn")]
    public Transform spawnPoint;
    public GameObject enemyPrefab;


    [Header("Score")]
    public int score = 0;

    [Header("UI Score")]
    public TextMeshProUGUI scoreText;

    [Header("Coins")]
    public int coins = 0;

    [Header("UI Coins")]
    public TextMeshProUGUI coinsText;

    [Header("UI")]
    public GameObject shopPanel;
    public GameObject defenseMenu;
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI enemiesText;

    

    private float timer;
    private bool waveRunning = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartPreparation();
    }

    void Update()
    {
        if (currentPhase == Phase.Combat)
        {
            HandleCombat();
        }

        UpdateUI();
    }

    // -------------------------
    // PREPARATION
    // -------------------------
    public void StartPreparation()
    {
        currentPhase = Phase.Preparation;

        waveRunning = false;
        timer = 0;

        UpdatePhaseUI();

        phaseText.text = "Phase de préparation";

        Debug.Log("Préparation vague " + waveIndex);
    }



    public void StartWave()
    {
        if (currentPhase != Phase.Preparation)
            return;

        if (ShopManager.Instance != null && ShopManager.Instance.IsBuilding())
        {
            Debug.Log("Impossible de démarrer : construction en cours");
            return;
        }

        StartCoroutine(StartCombat());
    }




    IEnumerator StartCombat()
    {
        currentPhase = Phase.Combat;

        UpdatePhaseUI();

        phaseText.text = "Phase de défense";

        enemiesRemainingToSpawn = GetEnemyCountForWave();
        enemiesAlive = 0;

        timer = waveDuration;

        waveRunning = true;

        StartCoroutine(SpawnEnemiesOverTime());

        yield return null;
    }

    // -------------------------
    // COMBAT
    // -------------------------
    void HandleCombat()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
            timer = 0f;

        if (enemiesRemainingToSpawn <= 0 && enemiesAlive <= 0)
        {
            EndWave();
        }
    }

    IEnumerator SpawnEnemiesOverTime()
    {
        int totalToSpawn = enemiesRemainingToSpawn;

        if (totalToSpawn <= 0)
            yield break;

        float spawnInterval = waveDuration / totalToSpawn;

        while (enemiesRemainingToSpawn > 0)
        {
            SpawnEnemy();

            enemiesRemainingToSpawn--;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
        {
            Debug.LogError("EnemyPrefab ou SpawnPoint manquant !");
            return;
        }

        GetWaveConfig(waveIndex, out int level, out int totalCount);

        bool isBoss = (enemiesRemainingToSpawn == 1); // dernier ennemi = boss

        EnemyStats stats = GetEnemyStats(level, isBoss);

        GameObject enemyObj = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        EnemyMovement enemy = enemyObj.GetComponent<EnemyMovement>();

        if (enemy != null)
        {
            enemy.maxHealth = stats.maxHealth;
            enemy.currentHealth = stats.maxHealth;
            enemy.speed = stats.speed;
            enemy.scoreValue = stats.scoreValue;
            enemy.reward = stats.reward;

            if (stats.isBoss)
            {
                enemy.transform.localScale *= 3f;
            }
        }

        enemyObj.SetActive(true);

        enemiesAlive++;
    }

    // -------------------------
    // NOTIFICATIONS ENNEMIS
    // -------------------------

    public void EnemyKilled(int scoreValue, int reward)
    {
        enemiesAlive--;
        score += scoreValue;
        coins += reward;
    }

    public void EnemyReachedEnd()
    {
        enemiesAlive--;
    }

    // -------------------------
    // FIN DE VAGUE
    // -------------------------
    void EndWave()
    {
        if (!waveRunning)
            return;

        waveRunning = false;

        waveIndex++;

        StartPreparation();
    }

    // -------------------------
    // CONFIG WAVES
    // -------------------------
    void GetWaveConfig(int wave, out int enemyLevel, out int enemyCount)
    {
        enemyLevel = 1 + (wave / 3); // level change toutes les 3 vagues
        enemyCount = 10 + (wave / 2); // nombre d'ennemis augmente toutes les 2 vagues
    }

    [System.Serializable]
    public struct EnemyStats
    {
        public float speed;
        public int maxHealth;
        public int scoreValue;
        public int reward;
        public bool isBoss;
    }

    EnemyStats GetEnemyStats(int level, bool isBoss)
    {
        EnemyStats stats = new EnemyStats();

        stats.speed = 1f + (level - 1) * 0.1f;
        stats.maxHealth = 50 + (level - 1) * 50;
        stats.scoreValue = 10 + (level - 1) * 5;
        stats.reward = 10 + (level - 1) * 5;
        stats.isBoss = isBoss;

        if (isBoss)
        {
            stats.maxHealth *= 5;
            stats.scoreValue *= 10;
            stats.reward *= 10;
        }

        return stats;
    }

    // -------------------------
    // UI
    // -------------------------
    void UpdateUI()
    {
        if (timerText != null)
            timerText.text = Mathf.Ceil(timer).ToString();

        if (multiplierText != null)
            multiplierText.text = "X" + multiplier.ToString("F1");

        int totalEnemiesRemaining = enemiesAlive + enemiesRemainingToSpawn;

        if (enemiesText != null)
            enemiesText.text = "Ennemis restants: " + totalEnemiesRemaining;

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (coinsText != null)
            coinsText.text = coins.ToString();
    }

    void UpdatePhaseUI()
    {
        bool isCombat = currentPhase == Phase.Combat;

        if (shopPanel != null)
            shopPanel.SetActive(!isCombat);

        if (defenseMenu != null)
            defenseMenu.SetActive(isCombat);
    }

    // -------------------------
    // CONFIG VAGUE
    // -------------------------
    int GetEnemyCountForWave()
    {
        return 10 + (waveIndex * 2);
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}