using System.Collections;
using UnityEngine;

namespace Utils
{
    public class Scheduler
    {
        /// <summary>
        /// Generic Coroutine to trigger a method at specified times.
        /// </summary>
        public static IEnumerator ScheduleMethod(float[] times, System.Action methodToRun)
        {
            foreach (float time in times)
            {
                float startTime = Time.time;
                while (Time.time < startTime + time)
                {
                    yield return null; // Wait for the next frame
                }
                methodToRun.Invoke(); // Call the method at the scheduled time
            }
        }
    }
}