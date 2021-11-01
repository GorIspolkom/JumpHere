using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HairyEngine.HairyCamera
{
    public enum EaseType
    {
        EaseInOut,
        EaseOut,
        EaseIn,
        Linear
    }
    public static class Utils
    {
        public static float EaseFromTo(float start, float end, float value, EaseType type = EaseType.EaseInOut)
        {
            value = Mathf.Clamp01(value);

            switch (type)
            {
                case EaseType.EaseInOut:
                    return Mathf.Lerp(start, end, value * value * (3.0f - 2.0f * value));

                case EaseType.EaseOut:
                    return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));

                case EaseType.EaseIn:
                    return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));

                default:
                    return Mathf.Lerp(start, end, value);
            }
        }
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return Mathf.Clamp((value - from1) / (to1 - from1) * (to2 - from2) + from2, from2, to2);
        }
        public static Vector3 NablaMultiply(this Vector3 v1, Vector3 v2)
        {
            v1.x *= v2.x;
            v1.y *= v2.y;
            v1.z *= v2.z;
            return v1;
        }
    }
}