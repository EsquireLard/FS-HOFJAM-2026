using UnityEngine;

public class GrassTile : Tile
{
    [SerializeField] private Color baseColor, offsetColor;

    public override void Init(int x, int y)
    {
        bool isOffset = (x + y) % 2 == 1;
        renderer.color = isOffset ? offsetColor : baseColor;
    }
}
    
