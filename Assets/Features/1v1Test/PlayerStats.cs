using System.Collections;
using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public FloatVariable cutAtk;
    public FloatVariable stabAtk;
    public FloatVariable cutDef;
    public FloatVariable stabDef;
    public FloatVariable health;
}
