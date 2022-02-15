using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using Utilities;

namespace Core.GC
{
    [CreateAssetMenu(fileName = "GCSystem", menuName = "Core/Game/Garbage Collection System", order = 3)]
    [DefaultExecutionOrder(-9999)]
    public class GCSystem : ScriptableObject
    {
        [ReadOnly] [SerializeField] bool GC_Enabled = true;
        [SerializeField] ulong incrementalGCTimeSlice = 3000000;

        [ContextMenu("Enable GC")]
        public void EnableGC()
        {
            GC_Enabled = true;
            GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
        }

        [ContextMenu("Disable GC")]
        public void DisableGC()
        {
            GC_Enabled = false;
            GarbageCollector.GCMode = GarbageCollector.Mode.Manual;
        }


        [ContextMenu("Set GC TimeSlice")]
        public void SetGCTimeSlice(ulong timeSlice = 0)
        {
            if (timeSlice == 0)
                timeSlice = incrementalGCTimeSlice;
            else
                incrementalGCTimeSlice = timeSlice;

            GarbageCollector.incrementalTimeSliceNanoseconds = timeSlice;
        }

        public bool CollectGC(ulong nanoseconds) => GarbageCollector.CollectIncremental(nanoseconds);
    }
}