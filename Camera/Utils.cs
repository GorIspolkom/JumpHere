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

    }
}
