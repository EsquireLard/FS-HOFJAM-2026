using System.Collections;
using UnityEngine;

public class monsters : MonoBehaviour, ITarget
{
    public enum spawnDir
    {
        North = 0,
        South,
        East,
        West
    }
    const float attkRate = 1.0f;

    [SerializeField] so_monsters stats;

    [SerializeField] bool debug_AttackCondition;
    [SerializeField] bool debug_TileOccupied;
    [SerializeField] GameObject debug_AttackTarget;

    spawnDir spawn;
    Tile dest;

    float curr_hp;
    float attkTimer;
    float dmgTimer;
    bool attkTimerStart;
    bool dot;
    bool dying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attkTimer = 0;
        dmgTimer = 0;
        attkTimerStart = false;
        curr_hp = stats.hp_;
        Set_Spawn_Direction();
    }

    // Update is called once per frame
    void Update()
    {
        if (attkTimerStart || attkTimer > 0)
        {
            attkTimer -= Time.deltaTime;
        }
        if (dmgTimer > 0)
        {
            dmgTimer -= Time.deltaTime;
        }
        if (debug_AttackCondition)
        {
            Attack(debug_AttackTarget);
        }
    }

    void Attack(GameObject target)
    {
        IDamage dmg = target.GetComponent<IDamage>();
        if (dmg != null)
        {
            if (attkTimer <= 0)
            {
                dmg.Take_Damage(stats.dmg_);
                attkTimer = attkRate;
            }
        }
    }

    void Set_Spawn_Direction()
    {
        if(transform.position.y > 10.0f)//north
        {
            spawn = spawnDir.North;
        }
        else if (transform.position.y < 0)//south
        {
            spawn = spawnDir.South;
        }
        else if (transform.position.x < 0)//west
        {
            spawn = spawnDir.West;
        }
        else//east
        {
            spawn = spawnDir.East;
        }
        Set_Destination();
    }

    void Set_Destination()
    {
        Vector2 currTilePos = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        Tile currTile;
        do
        {
            switch(spawn)
            {
                case spawnDir.North:
                    currTilePos.y -= 1.0f;
                    break;

                case spawnDir.South:
                    currTilePos.y += 1.0f;
                    break;

                case spawnDir.West:
                    currTilePos.x += 1.0f;
                    break;

                case spawnDir.East:
                    currTilePos.x -= 1.0f;
                    break;
                default:
                    break;
            }
            currTile = TileManager.instance.GetTileAtPosition(currTilePos);
            if (debug_TileOccupied)
            {
                dest = currTile;
            }
        } while (dest = null);
    }

    public void Lured()
    {
        
    }

    public void Take_Damage(int amount)
    {
        curr_hp -= amount;
        Debug.Log("a monster just took damage");
        if (curr_hp <= 0 && !dying)
        {
            Die();
        }
    }

    public void Take_Damage(int amount, float rate, float timeDOT)
    {
        dmgTimer = timeDOT;
        if (!dot)
        {
            StartCoroutine(DOT(amount, rate));
        }
    }

    public void Die()
    {
        dying = true;
        Debug.Log("a monster just died");
    }

    IEnumerator DOT(int amount, float rate)
    {
        dot = true;
        while (dmgTimer > 0 && !dying)
        {
            curr_hp -= amount;
            Debug.Log("a monster just took damage");
            if (curr_hp <= 0 && !dying)
            {
                Die();
            }
            yield return new WaitForSeconds(rate);
        }
        dot = false;
    }
}
