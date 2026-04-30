using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public GameObject towerMenu;
    public GameObject shopMenu;

    [Header("Tour sélectionnée")]
    public Tower selectedTower;

    [Header("Slots disponibles")]
    public int availableSlots = 5;

    private BuildSlot[] allSlots;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        allSlots = FindObjectsOfType<BuildSlot>();

        Debug.Log("Nombre de slots trouvés : " + allSlots.Length);
    }

    public bool IsBuilding()
    {
        return selectedTower != null;
    }

    public bool HasFreeSlots()
    {
        return availableSlots > 0;
    }


    public void SelectTower(Tower tower)
    {
   
        if (selectedTower != null)
        {
            Debug.Log("Construction déjà en cours");
            return;
        }

        if (availableSlots <= 0)
        {
            Debug.Log("Aucun slot disponible !");
            return;
        }

        selectedTower = tower;

        Debug.Log("Mode construction ACTIVÉ pour : " + tower.name);

        ShowAllSlots();
    }

    public void OnTowerPlaced()
    {
        availableSlots--;
        selectedTower = null;

        HideAllSlots();
    }

    public void CancelSelection()
    {

        selectedTower = null;

        HideAllSlots();
    }


    void ShowAllSlots()
    {

        foreach (BuildSlot slot in allSlots)
        {
            slot.ShowHighlight();
        }
    }

    void HideAllSlots()
    {

        foreach (BuildSlot slot in allSlots)
        {
            slot.HideHighlight();
        }
    }

    public void closeTowerMenu()
    {
        if (towerMenu != null)
        {
            towerMenu.SetActive(false);
            shopMenu.SetActive(true);
        }
    }
}