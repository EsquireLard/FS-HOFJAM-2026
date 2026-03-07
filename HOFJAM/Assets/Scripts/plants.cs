using Unity.VisualScripting;
using UnityEngine;

public class plants : MonoBehaviour, IDamage
{
    [SerializeField] so_plants stats;
    [SerializeField] bool debug_AttackCondition;
    [SerializeField] GameObject debug_Target;

    bool dmgTimerStart;
    float dmgTimer;
    bool dragging;

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
            var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePosition);
            transform.position = mousePosition;
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

    void OnMouseDown()
    {
        dragging = true;
        GameManager.instance.SetSelectedUnit(this);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
    void OnMouseUp()
    {
        dragging = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        if (GameManager.instance.hoverTile != null)
        {
            GameManager.instance.hoverTile.SetUnit(this);
        }
        GameManager.instance.SetSelectedUnit(null);

    }
}
