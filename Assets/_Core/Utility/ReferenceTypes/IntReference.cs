using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utilities
{
    [CreateAssetMenu(fileName = "IntReference", menuName = "Utilities/Reference Data/Int", order = 3)]
    public class IntReference : ScriptableObject
    {
        [SerializeField] int value = 0;
        public int Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(this.value);
            }
        }

        public event System.Action<int> OnValueChanged;

        public void SetInt(int value) => Value = value;
    }
}