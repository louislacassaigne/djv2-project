using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

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

    // -------------------------
    // SÉLECTION TOUR
    // -------------------------
    public void SelectTower(Tower tower)
    {
        // ❌ déjà en construction
        if (selectedTower != null)
        {
            Debug.Log("Construction déjà en cours");
            Debug.Log("slots restants: " + availableSlots);
            return;
        }

        // ❌ plus de slots
        if (availableSlots <= 0)
        {
            Debug.Log("Aucun slot disponible !");
            return;
        }

        selectedTower = tower;

        Debug.Log("Mode construction ACTIVÉ pour : " + tower.name);

        ShowAllSlots();
    }

    // -------------------------
    // PLACEMENT FINAL
    // -------------------------
    public void OnTowerPlaced()
    {
        availableSlots--;

        Debug.Log("Tour placée. Slots restants: " + availableSlots);

        selectedTower = null;

        HideAllSlots();
    }

    public void CancelSelection()
    {
        Debug.Log("Mode construction DÉSACTIVÉ");

        selectedTower = null;

        HideAllSlots();
    }

    // -------------------------
    // VISUALISATION
    // -------------------------
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
}