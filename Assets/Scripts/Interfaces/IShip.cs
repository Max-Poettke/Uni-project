using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShip
{
    void Move();
    void tryGetInfo();
    void Die();
    void Stun(float duration);
    
    bool IsStunned { get; }
}
