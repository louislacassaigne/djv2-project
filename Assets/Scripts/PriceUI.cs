using TMPro;
using UnityEngine;

public class TowerShopUI : MonoBehaviour
{
    [Header("Référence tour")]
    public Tower tower;

    private TextMeshProUGUI priceText;

    void Awake()
    {
        priceText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (tower == null || priceText == null)
            return;

        priceText.text = tower.purchaseCost.ToString();
    }
}