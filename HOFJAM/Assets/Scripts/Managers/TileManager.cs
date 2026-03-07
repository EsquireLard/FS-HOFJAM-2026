using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    [SerializeField] int width, height;
    [SerializeField] Tile grassTile, fertileTile, cauldronTile, irrigationTile;
    [SerializeField] List<Vector2> fertileTileList, cauldronTileList, irrigationTileList;


    private Dictionary<Vector2, Tile> tiles;

    void Awake()
    {
        instance = this;
    }

    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile tileToSpawn;
                Vector2 currentTile = new Vector2(x, y);
                if (fertileTileList.Contains(currentTile))
                {
                    tileToSpawn = fertileTile;
                }
                else if (cauldronTileList.Contains(currentTile))
                {
                    tileToSpawn = cauldronTile;
                }
                else if (irrigationTileList.Contains(currentTile))
                {
                    tileToSpawn = irrigationTile;
                }
                else
                {
                    tileToSpawn = grassTile;
                }

                    Tile spawnedTile = Instantiate(tileToSpawn, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.Init(x, y);

                tiles[new Vector2 (x, y)] = spawnedTile;
            }
        }

        GameManager.instance.cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

        GameManager.instance.ChangeState(GameState.Running);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }

        return null;
    }
}
