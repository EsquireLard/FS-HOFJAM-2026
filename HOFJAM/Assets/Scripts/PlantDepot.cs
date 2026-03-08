using UnityEngine;

public class PlantDepot : MonoBehaviour
{
    [SerializeField] public plants plantPrefab;
    plants lastPlant;

    public void GrabPlant()
    {
        if (GameManager.instance.selectedPlant == null)
        {
            lastPlant = Instantiate(plantPrefab, GameManager.instance.mousePosition, Quaternion.identity);
        }
        else if (GameManager.instance.selectedPlant != null)
        {
            GameManager.instance.selectedPlant.OnMouseUp();
        }
    }

    public void DragPlant()
    {
        if (lastPlant != null)
        {
            if (lastPlant.currentTile == null)
            {
                lastPlant.OnMouseDown();
            }
        }
    }

    public void ReleasePlant()
    {
        if (lastPlant != null && GameManager.instance.selectedPlant == lastPlant)
        {
            lastPlant.OnMouseUp();
            lastPlant = null;
        }
    }
}
