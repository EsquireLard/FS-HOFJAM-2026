using UnityEngine;

public class monsters : MonoBehaviour, ITarget
{
    [SerializeField] so_monsters stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lured()
    {
        throw new System.NotImplementedException();
    }

    public void Take_Damage(int amount)
    {
        throw new System.NotImplementedException();
    }

    public void Take_Damage(int amount, float rate, float timeDOT)
    {
        throw new System.NotImplementedException();
    }
}
