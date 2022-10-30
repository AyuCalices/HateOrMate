using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityType_SO : ScriptableObject
{
    public EntityType type;
}

public enum EntityType
{
    Player,
    Enemy
}
