using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public Tower towerToBuy;

    public void BuyTower()
    {
        Debug.Log("Bouton BUY cliqué");

        if (towerToBuy == null)
        {
            Debug.LogError("towerToBuy est NULL");
            return;
        }

        ShopManager.Instance.SelectTower(towerToBuy);
    }
}