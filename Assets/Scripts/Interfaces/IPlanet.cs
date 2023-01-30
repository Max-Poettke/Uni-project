using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IPlanet : IHp
{
    void FireProjectile();
    void SetPhase(int i);
    int GetPhase();
    void SetSlider(Slider slider);
    void OneThird();
    void TwoThirds();
}
