using UnityEngine;

namespace Farm
{
    public static class Vector3Extension
    {
        public static Vector3 Absolute(this Vector3 oldVector)
        {
            Vector3 newVector = new Vector3
            (
                Mathf.Abs(oldVector.x),
                Mathf.Abs(oldVector.y),
                Mathf.Abs(oldVector.z)
            );

            return newVector;
        }

        public static Vector3 Clamp(this Vector3 oldVector, Vector3 clamp)
        {
            Vector3 newVector = new Vector3
            (
                Mathf.Clamp(oldVector.x, -clamp.x, clamp.x),
                Mathf.Clamp(oldVector.y, -clamp.y, clamp.y),
                Mathf.Clamp(oldVector.z, -clamp.z, clamp.z)
            );

            return newVector;
        }

        public static Vector3 Multiply(this Vector3 oldVector, Vector3 factor)
        {
            Vector3 newVector = new Vector3
            (
                oldVector.x * factor.x,
                oldVector.y * factor.y,
                oldVector.z * factor.z
            );

            return newVector;
        }
    }
}
