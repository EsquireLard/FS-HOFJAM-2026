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
    [SerializeField] GameObject prefab_;
    [SerializeField] bool killable_;
    [SerializeField] bool attracts_;

    [SerializeField] attkType attk_;

    [SerializeField] int hp_;
    [SerializeField] int dmg_;
}
