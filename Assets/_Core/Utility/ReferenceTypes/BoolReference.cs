using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    [CreateAssetMenu(fileName = "BoolReference", menuName = "Utilities/Reference Data/Bool", order = 1)]
    public class BoolReference : ScriptableObject
    {
        [SerializeField] bool value = false;
        public bool Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(this.value);
            }
        }

        public event System.Action<bool> OnValueChanged;

        public void SetTrue() => Value = true;
        public void SetFalse() => Value = false;
    }
}