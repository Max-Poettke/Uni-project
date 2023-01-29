using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlanet : IHp
{
    void FireProjectile();
    void SetPhase(int i);
    int GetPhase();
    void OneThird();
    void TwoThirds();
}
