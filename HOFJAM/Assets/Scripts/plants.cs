using Unity.VisualScripting;
using UnityEngine;

public class plants : MonoBehaviour, IDamage
{
    [SerializeField] so_plants stats;
    [SerializeField] bool debug_AttackCondition;
    [SerializeField] GameObject debug_Target;

    bool dmgTimerStart;
    float dmgTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dmgTimer = 0;
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
}
