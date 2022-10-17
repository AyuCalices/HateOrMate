using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace DataStructures.RuntimeSet
{
    public abstract class RuntimeSet_SO<T> : ScriptableObject
    {
        [SerializeField, ReadOnly] private HashSet<T> items = new HashSet<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }
    }
}
