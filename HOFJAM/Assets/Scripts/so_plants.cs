using UnityEngine;

[CreateAssetMenu]
public class so_plants : ScriptableObject
{
    public enum attkType
    {
        None = 0,
        Thorns,
        Poison,
        Ranged
    }
    [SerializeField] public GameObject prefab_;
    [SerializeField] public bool killable_;
    [SerializeField] public bool attracts_;

    [SerializeField] public attkType attk_;

    [SerializeField] public int hp_;
    [SerializeField] public int dmg_;
    [SerializeField] public int range_;
    [SerializeField] public float attkRate_;
    [SerializeField] public float dmgrate_;
    [SerializeField] public float dmgTime_;
}
