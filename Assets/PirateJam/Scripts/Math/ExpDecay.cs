using UnityEngine;

namespace PirateJam.Scripts.Math
{
    public static class MathUtil
    {
        public static Vector2 Vector2Decay(Vector2 a, Vector2 b, float decay, float dt)
        {
            return b + (a - b) * Mathf.Exp(-decay * dt);
        }

        public static float FloatDecay(float a, float b, float decay, float dt)
        {
            return b + (a - b) * Mathf.Exp(-decay * dt);
        }
    }
}