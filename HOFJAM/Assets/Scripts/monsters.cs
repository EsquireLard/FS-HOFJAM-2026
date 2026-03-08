using System.Collections;
using Unity.VisualScripting;
using Unity.XR.Oculus.Input;
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
    const float moveSpeed = 1.0f;

    [SerializeField] so_monsters stats;

    [SerializeField] bool debug_AttackCondition;
    [SerializeField] bool debug_TileOccupied;
    IDamage debug_AttackTarget;

    spawnDir spawn;
    Tile dest;

    float curr_hp;
    float attkTimer;
    float dmgTimer;
    bool attkTimerStart;
    bool dot;
    bool dying;
    bool spawning;
    bool lured;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attkTimer = 0;
        dmgTimer = 0;
        attkTimerStart = false;
        spawning = true;
        lured = false;
        curr_hp = stats.hp_;
        StartCoroutine(SETSPAWNDIR());
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
        if (!spawning)
        {
            if (debug_AttackCondition || Destination_Check())
            {
                Attack(debug_AttackTarget);
            }
            else
            {
                Move();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered Trigger");
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            debug_AttackCondition = true;
        }
    }

    void Move()
    {
        Vector2 moveDir = Vector2.zero;
        if (lured)
        {
            moveDir = (transform.position - dest.transform.position).normalized;
            if (Destination_Check())
            {
                lured = false;
                Set_Destination();
            }
        }
        else
        {
            switch (spawn)
            {
                case spawnDir.North:
                    moveDir = Vector2.down;
                    break;
                case spawnDir.South:
                    moveDir = Vector2.up;
                    break;
                case spawnDir.West:
                    moveDir = Vector2.right;
                    break;
                case spawnDir.East:
                    moveDir = Vector2.left;
                    break;
            }
        }
        transform.Translate(moveDir.x * moveSpeed * Time.deltaTime, moveDir.y * moveSpeed * Time.deltaTime, 0.0f);
    }

    void Attack(IDamage target)
    {
        if (debug_AttackTarget != null)
        {
            if (attkTimer <= 0)
            {
                target.Take_Damage(stats.dmg_);
                attkTimer = attkRate;
            }
        }
    }

    void Set_Spawn_Direction()
    {
        if(transform.position.y == 9.0f)//north
        {
            spawn = spawnDir.North;
        }
        else if (transform.position.y == -1.0f)//south
        {
            spawn = spawnDir.South;
        }
        else if (transform.position.x == -1.0f)//west
        {
            spawn = spawnDir.West;
        }
        else//east
        {
            spawn = spawnDir.East;
        }
        Set_Destination();
        spawning = false;
    }

    void Set_Destination()
    {
        Vector2 currTilePos = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        Tile currTile;
        bool lastTile = false;
        do
        {
            switch(spawn)
            {
                case spawnDir.North:
                    currTilePos.y -= 1.0f;
                    if(currTilePos.y == 3.0f)
                    {
                        lastTile = true;
                    }
                    break;

                case spawnDir.South:
                    currTilePos.y += 1.0f;
                    if(currTilePos.y == 5.0f)
                    {
                        lastTile = true;
                    }
                    break;

                case spawnDir.West:
                    currTilePos.x += 1.0f;
                    if(currTilePos.x == 5.0f)
                    {
                        lastTile = true;
                    }
                    break;

                case spawnDir.East:
                    currTilePos.x -= 1.0f;
                    if(currTilePos.x == 3.0f)
                    {
                        lastTile = true;
                    }
                    break;
                default:
                    break;
            }
            currTile = TileManager.instance.GetTileAtPosition(currTilePos);
            if (lastTile)
            {
                dest = currTile;
            }
        } while (dest == null);
    }

    bool Destination_Check()
    {
        if (Mathf.Abs((dest.transform.position.x - transform.position.x)) <= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Lured(GameObject target)
    {
        Vector2 targetPos = new Vector2(Mathf.Floor(target.transform.position.x), Mathf.Floor(target.transform.position.y));
        dest = TileManager.instance.GetTileAtPosition(targetPos);
        lured = true;
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

    IEnumerator SETSPAWNDIR()
    {
        yield return new WaitForSeconds(1.0f);
        Set_Spawn_Direction();
    }
}
