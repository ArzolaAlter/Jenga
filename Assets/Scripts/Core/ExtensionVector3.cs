using UnityEngine;

namespace Jenga
{
    public static class ExtensionVector3
    {
        public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Vector3 axis, float angle)
        {
            return Quaternion.AngleAxis(angle, axis) * (point - pivot) + pivot;
        }
    }
}