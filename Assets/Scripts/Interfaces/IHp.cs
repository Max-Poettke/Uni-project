using System.Collections;
using System.Collections.Generic;using UnityEditor;
using UnityEngine;

public interface IHp : IKillable 
{
    void TakeDamage(float damage, float armorPenetrationFactor);
}
