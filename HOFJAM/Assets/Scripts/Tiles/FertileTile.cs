using UnityEngine;

public class FertileTile : Tile
{
    [SerializeField] private Color baseColor, offsetColor;

    public override void Init(int x, int y)
    {
        bool isOffset = (x + y) % 2 == 1;
        renderer.color = isOffset ? offsetColor : baseColor;
    }
}
