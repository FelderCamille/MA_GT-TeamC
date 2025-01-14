using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static float ToLog(float value)
        {
            return Mathf.Log10(value) * 20;
        }
    }
}