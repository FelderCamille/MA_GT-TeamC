using UnityEngine.Events;

namespace Utils
{
    public static class EventManager
    {
        public static event UnityAction TimerStart;
        public static event UnityAction TimerStop;

        public static void OnTimerStart() => TimerStart?.Invoke();
        public static void OnTimerStop() => TimerStop?.Invoke();
    }
}