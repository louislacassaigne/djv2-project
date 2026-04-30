using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public Tower towerToBuy;

    public void BuyTower()
    {
        if (WaveManager.Instance.coins < towerToBuy.purchaseCost)
        {
            Debug.Log("Pas assez de pièces pour acheter cette tour !");
            return;
        }
        else
        {
            WaveManager.Instance.coins -= towerToBuy.purchaseCost;
            ShopManager.Instance.SelectTower(towerToBuy);
            WaveManager.Instance.UpdateUI();
        }

    }
}