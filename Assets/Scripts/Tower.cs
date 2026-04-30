using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tower : MonoBehaviour


{
    public int level = 1;

    [Header("Menus")]

    public GameObject shopMenu;
    public GameObject towerMenu;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI targetsText;
    public TextMeshProUGUI upgradeCostText;
    public Sprite icon;
    public Image towerImage;

    [Header("Économie")]
    public int purchaseCost = 500;

    [Header("Tir")]
    public int projectileCount = 1;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    public int projectileDamage = 10;
    public float range = 8f;

    [Header("Cibles détectées")]
    public List<EnemyMovement> detectedEnemies = new List<EnemyMovement>();


    void Update()
    {
        DetectEnemies();
    }

    void DetectEnemies()
    {
        EnemyMovement[] allEnemies = FindObjectsOfType<EnemyMovement>();

        List<EnemyMovement> enemiesInRange = allEnemies
            .Where(enemy =>
                Vector2.Distance(
                    new Vector2(transform.position.x, transform.position.z),
                    new Vector2(enemy.transform.position.x, enemy.transform.position.z)
                ) <= range
            )
            .ToList();

        enemiesInRange = enemiesInRange
            .OrderBy(enemy => enemy.transform.position.z)
            .ToList();

        detectedEnemies = enemiesInRange
            .Take(projectileCount)
            .ToList();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void OnMouseDown()
    {
        if (!ShopManager.Instance.IsBuilding() && WaveManager.Instance.currentPhase == WaveManager.Phase.Preparation)
        {
            shopMenu.SetActive(false);
            towerMenu.SetActive(true);
            UpdateTowerMenu();
        }
    }

    public void UpdateTowerMenu()
    {
        towerMenu.GetComponent<TowerMenu>().selected_tower = this;
        damageText.text = "Dégâts: " + projectileDamage;
        fireRateText.text = "Cadence de tir: " + fireRate.ToString("F1") + " tirs/s";
        rangeText.text = "Portée: " + range.ToString("F1");
        levelText.text = "Niveau: " + level;
        targetsText.text = "Cibles: " + projectileCount;
        upgradeCostText.text = (level*200).ToString();
        towerImage.sprite = icon;
    }

    public void Upgrade()
{
    level++;
    fireRate += 0.05f;
    projectileDamage += 5;
    UpdateTowerMenu();
}


}