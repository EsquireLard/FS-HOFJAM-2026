using Unity.VisualScripting;
using UnityEngine;

public class plants : MonoBehaviour, IDamage
{
    [SerializeField] so_plants stats;
    [SerializeField] bool debug_AttackCondition;
    [SerializeField] GameObject debug_Target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

                    break;

                case so_plants.attkType.Poison:

                    break;

                case so_plants.attkType.Ranged:

                    break;
            }
        }
    }

    void Lure(GameObject target)
    {

    }

    public void Take_Damage()
    {
        
    }
}
