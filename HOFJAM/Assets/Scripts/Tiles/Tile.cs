using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;

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
}
