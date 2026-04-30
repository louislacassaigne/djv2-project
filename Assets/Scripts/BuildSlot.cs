using UnityEngine;

public class BuildSlot : MonoBehaviour
{
    [Header("Visual")]
    public GameObject highlight;

    private bool occupied = false;

    void Start()
    {
        HideHighlight();
    }

    void OnMouseDown()
    {
        Debug.Log("Slot cliqué : " + gameObject.name);

        // Slot déjà utilisé
        if (occupied)
        {
            Debug.Log("Slot déjà occupé");
            return;
        }

        // Vérifie qu'une tour est sélectionnée
        if (ShopManager.Instance.selectedTower == null)
        {
            Debug.Log("Aucune tour sélectionnée");
            return;
        }

        BuildTower();
    }

    void BuildTower()
    {
        Debug.Log("Construction de la tour");

        GameObject towerObject =
            ShopManager.Instance.selectedTower.gameObject;

        GameObject clone = Instantiate(
            towerObject,
            transform.position,
            Quaternion.identity
        );

        clone.SetActive(true);

        occupied = true;

        if (highlight != null)
        {
            Destroy(highlight);
        }

        ShopManager.Instance.OnTowerPlaced();
        ShopManager.Instance.CancelSelection();
    }

    public void ShowHighlight()
    {
        // Si déjà construit, ne rien afficher
        if (occupied)
            return;

        if (highlight != null)
        {
            highlight.SetActive(true);
        }
    }

    public void HideHighlight()
    {
        if (highlight != null)
        {
            highlight.SetActive(false);
        }
    }
}