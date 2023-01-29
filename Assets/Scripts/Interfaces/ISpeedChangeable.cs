using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpeedChangeable
{
    void SetSpeed(float newSpeed);
    void SetSpeed(float minSpeed, float maxSpeed);
    float GetSpeed();
}
