using System;
using Unity.Collections;
using UnityEngine;

namespace DataStructures.Variables
{
    [Serializable]
    public abstract class AbstractReference<T>
    {
        [SerializeField, ReadOnly] private bool useOverride;
        [SerializeField, ReadOnly] private T overrideValue;
        [SerializeField] private AbstractVariable<T> variable;

        protected AbstractReference(T value)
        {
            useOverride = true;
            overrideValue = value;
        }

        public void SetOverride(T value)
        {
            useOverride = true;
            overrideValue = value;
        }

        public void ResetOverride()
        {
            useOverride = false;
        }
        
        protected T value => useOverride ? overrideValue : variable.Get();

        public static implicit operator T(AbstractReference<T> reference)
        {
            return reference.value;
        }
    }
}