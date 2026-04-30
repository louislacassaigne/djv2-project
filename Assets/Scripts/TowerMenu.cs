using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    public Tower selected_tower;

    public void Upgrade()
    {
        if (WaveManager.Instance.coins < selected_tower.level * 200)
        {
            Debug.Log("Pas assez de pièces pour améliorer cette tour !");
            return;
        }
        else
        {
            WaveManager.Instance.coins -= selected_tower.level * 200;
            WaveManager.Instance.UpdateUI();
            selected_tower.Upgrade();
        }
    }
}
