using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    int dmgUpgrade { get; set; }
    int firingUpgrade { get; set; }
    int penetrationUpgrade { get; set;}
    void Fire();
    void Upgrade();
}
