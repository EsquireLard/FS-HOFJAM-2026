using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;
    [SerializeField] public bool plantable;

    public plants plantOnTile;

    public virtual void Init(int x, int y)
    {
    }

    void OnMouseEnter()
    {
        highlight.SetActive(true);
        GameManager.instance.hoverTile = this;
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
        GameManager.instance.hoverTile = null;
    }

    void OnMouseDown()
    {
        if (GameManager.instance.selectedPlant != null)
        {
            GameManager.instance.selectedPlant.OnMouseUp();
        }
    }

    public void SetUnit(plants unit)
    {
        if (unit.currentTile != null) unit.currentTile.plantOnTile = null;
        unit.currentTile = this;
        unit.transform.position = transform.position;
        plantOnTile = unit;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
