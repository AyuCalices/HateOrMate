using System.Collections;
using System.Collections.Generic;
using DataStructures.Event;
using DataStructures.Variables;
using UnityEngine;

[CreateAssetMenu]
public class Entity : ScriptableObject
{
    public string name;
    //stats
    public float attackTime;
    public FloatVariable atk;
    public FloatVariable def;
    public FloatVariable totalHealth;
    public FloatVariable currentHealth;
    public IntVariable totalStamina;
    public IntVariable currentStamina;
    public float staminaRefreshTime;
}
