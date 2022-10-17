using System;
using DataStructures.Event;
using UnityEngine;

namespace DataStructures.Variables
{
    public abstract class AbstractVariable<T> : ScriptableObject
    {
        [SerializeField] protected T runtimeValue;
        [SerializeField] protected T storedValue;
        [SerializeField] protected ActionEventWithParameter<T> onValueChanged;

        private void OnEnable()
        {
            Restore();
        }

        public void RegisterOnValueChangedAction(Action<T> action)
        {
            onValueChanged.RegisterListener(action);
        }
        
        public void UnregisterOnValueChangedAction(Action<T> action)
        {
            onValueChanged.UnregisterListener(action);
        }

        public void Restore()
        {
            runtimeValue = storedValue;
            if(onValueChanged != null) onValueChanged.Raise(runtimeValue);
        }

        public T Get() => runtimeValue;

        public void Set(T value)
        {
            if (value.Equals(runtimeValue)) return;
            
            runtimeValue = value;
            if(onValueChanged != null) onValueChanged.Raise(runtimeValue);
        }

        public void Copy(AbstractVariable<T> other) => runtimeValue = other.runtimeValue;
    }
}
