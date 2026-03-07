using UnityEngine;

[CreateAssetMenu]
public class so_monsters : ScriptableObject
{
    [SerializeField] public GameObject prefab_;
    [SerializeField] public int hp_;
    [SerializeField] public int dmg_;
}
