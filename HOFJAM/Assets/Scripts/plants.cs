using Unity.VisualScripting;
using UnityEngine;

public class plants : MonoBehaviour, IDamage
{
    [SerializeField] so_plants stats;
    [SerializeField] bool debug_AttackCondition;
    [SerializeField] GameObject debug_Target;

    bool dmgTimerStart;
    float dmgTimer;
    public bool dragging;

    public Tile currentTile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dmgTimer = 0;
        dmgTimerStart = false;

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
        target.Lured();
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
