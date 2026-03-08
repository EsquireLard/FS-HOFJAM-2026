using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class plants : MonoBehaviour, IDamage
{
    [SerializeField] so_plants stats;
    [SerializeField] bool debug_AttackCondition;
    [SerializeField] GameObject debug_Target;

    bool dmgTimerStart;
    float dmgTimer;
    int rangeCurr;
    List<Tile> attack_tiles;
    List<Tile> lure_tiles;

    public bool dragging;

    public Tile currentTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dmgTimer = 0;
        dmgTimerStart = false;
        rangeCurr = stats.range_;

        dragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dmgTimerStart || dmgTimer > 0)
        {
            dmgTimer -= Time.deltaTime;
        }
        if (debug_AttackCondition)
        {
            Attack(debug_Target);
        }

        if (dragging)
        {
            transform.position = GameManager.instance.mousePosition;
        }
    }

    void Generate_Target_Lists()
    {
        attack_tiles.Clear();
        lure_tiles.Clear();
        Vector2 currPos = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        Vector2 addPos = currPos;

        switch(stats.attk_)
        {
            case so_plants.attkType.Thorns:
                attack_tiles.Add(currentTile);
                break;

            case so_plants.attkType.Ranged:
                //north
                addPos.y += 1.0f;
                for (int i = 1; i < rangeCurr; ++i)
                {
                    addPos.y += 1.0f;
                    attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
                }
                //south
                addPos = currPos;
                addPos.y -= 1.0f;
                for (int i = 1; i < rangeCurr; ++i)
                {
                    addPos.y -= 1.0f;
                    attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
                }
                //east
                addPos = currPos;
                addPos.x += 1.0f;
                for (int i = 1; i < rangeCurr; ++i)
                {
                    addPos.x += 1.0f;
                    attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
                }
                //west
                addPos = currPos;
                addPos.x -= 1.0f;
                for (int i = 1; i < rangeCurr; ++i)
                {
                    addPos.x -= 1.0f;
                    attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
                }
                break;

            case so_plants.attkType.Poison:
                //north
                for (int i = 1; i < rangeCurr; ++i)
                {
                    addPos.y += 1.0f;
                    attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
                }
                //south
                addPos = currPos;
                for (int i = 1; i < rangeCurr; ++i)
                {
                    addPos.y -= 1.0f;
                    attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
                }
                //east
                addPos = currPos;
                for (int i = 1; i < rangeCurr; ++i)
                {
                    addPos.x += 1.0f;
                    attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
                }
                //west
                addPos = currPos;
                for (int i = 1; i < rangeCurr; ++i)
                {
                    addPos.x -= 1.0f;
                    attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
                }
                break;

            case so_plants.attkType.None:
                break;

            default:
                break;
        }

        if (stats.attracts_)
        {
            //north
            addPos.y += 1.0f;
            attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));

            //south
            addPos = currPos;
            addPos.y -= 1.0f;
            attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));

            //east
            addPos = currPos;
            addPos.x += 1.0f;
            attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));

            //west
            addPos = currPos;
            addPos.x -= 1.0f;
            attack_tiles.Add(TileManager.instance.GetTileAtPosition(addPos));
        }
    }

    void Attack(GameObject target)
    {
        ITarget tgt = debug_Target.GetComponent<ITarget>();
        if (tgt != null)
        {
            switch (stats.attk_)
            {
                case so_plants.attkType.None:
                    break;

                case so_plants.attkType.Thorns:
                    if (dmgTimer <= 0)
                    {
                        tgt.Take_Damage(stats.dmg_);
                        dmgTimer = stats.attkRate_;
                    }
                    break;

                case so_plants.attkType.Poison:
                    tgt.Take_Damage(stats.dmg_, stats.dmgrate_, stats.dmgTime_);
                    break;

                case so_plants.attkType.Ranged:
                    if (dmgTimer <= 0)
                    {
                        tgt.Take_Damage(stats.dmg_);
                        dmgTimer = stats.attkRate_;
                    }
                    break;
            }
            if (stats.attracts_)
            {
                Lure(tgt);
            }
        }
    }

    void Lure(ITarget target)
    {
        target.Lured(this.gameObject);
    }

    public void Take_Damage(int amount)
    {
        
    }

    public void Die()
    {

    }

    public void OnMouseDown() // Picking up plant
    {
        if (GameManager.instance.selectedPlant != null)
        {
            GameManager.instance.selectedPlant.OnMouseUp();
            return;
        }
        dragging = true;
        GameManager.instance.SetSelectedUnit(this);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    public void OnMouseUp() // Attempting to drop plant
    {
        dragging = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        if (GameManager.instance.hoverTile != null &&
            GameManager.instance.hoverTile.plantable &&
            GameManager.instance.hoverTile.plantOnTile == null) // Place plant on tile if matching criteria
        {
            if (currentTile != null) currentTile.GetComponent<BoxCollider2D>().enabled = true;
            GameManager.instance.hoverTile.SetUnit(this);
            Generate_Target_Lists();
        }
        else if (currentTile != null) // Return plant back to previous tile if not matching criteria
        {
            transform.position = currentTile.transform.position;
        }
        else // Destroying the plant object if it didn't have a previous tile (was instatiated)
        {
            Destroy(gameObject);
        }
            GameManager.instance.SetSelectedUnit(null);
    }
}
