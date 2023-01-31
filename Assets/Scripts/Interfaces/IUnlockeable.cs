using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnlockeable
{
    float GetPrice();
    void SetUnlocked();
    bool GetUnlocked();
}
