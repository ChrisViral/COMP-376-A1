using System;
using UnityEngine;

namespace A1
{
    [Serializable]
    public struct Bounds
    {
        [SerializeField]
        private float xMin, xMax;
        [SerializeField]
        private float zMin, zMax;

        public Vector3 BoundVector(Vector3 v) => new Vector3(Mathf.Clamp(v.x, this.xMin, this.xMax), 0f, Mathf.Clamp(v.z, this.zMin, this.zMax));
    }
}
