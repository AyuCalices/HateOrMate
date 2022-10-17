using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ClickerComponent : ScriptableObject
{
    public Entity targetUnit;
    public List<Entity> attackerUnits;

    public float baseClickerDamage;
}
