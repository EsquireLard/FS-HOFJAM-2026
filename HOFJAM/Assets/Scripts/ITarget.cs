using UnityEngine;

public interface ITarget
{
    void Take_Damage(int amount);
    void Take_Damage(int amount, float rate, float timeDOT);
    void Lured();
}
