using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;

    plants plantOnTile;

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

    void SetUnit(plants unit)
    {
        unit.transform.position = transform.position;
        plantOnTile = unit;
        // UN-COMMENT THE FOLLOWING WHENEVER PLANTS HOLD TILE INFO FOR THE TILE THEY ARE ON
        // if (unit.occupyingTile != null) unit.occupyingTile.plantOnTile = null;
        // unit.occupyingTile = this;
    }
}
