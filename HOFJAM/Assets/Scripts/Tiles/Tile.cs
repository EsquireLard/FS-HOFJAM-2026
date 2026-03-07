using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;

    public plants plantOnTile;

    public virtual void Init(int x, int y)
    {
    }

    void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    void OnMouseDown()
    {
        if (GameManager.instance.GameState != GameState.Running)
        {
            return;
        }

    }

    void OnMouseUp()
    {
        Debug.Log("TileMouseUpTrigger");

        if (GameManager.instance.selectedPlant != null)
        {
            plantOnTile = GameManager.instance.selectedPlant;
            GameManager.instance.selectedPlant.transform.position = transform.position;
        }
    }

    void OnMouseOver()
    {
        Debug.Log("tileHover");
        GameManager.instance.hoverTile = this;
    }

    public void SetUnit(plants unit)
    {
        if (unit.currentTile != null) unit.currentTile.plantOnTile = null;
        unit.currentTile = this;
        unit.transform.position = transform.position;
        plantOnTile = unit;
    }
}
