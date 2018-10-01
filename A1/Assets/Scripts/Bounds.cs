using System;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// 2D rectangular boundaries limiter
    /// </summary>
    [Serializable]
    public struct Bounds
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float xMin, xMax;   //X axis bounds
        [SerializeField]
        private float zMin, zMax;   //Z axis bounds
        #endregion

        #region Methods
        /// <summary>
        /// Clamps the given Vector3 inside this bounds object
        /// </summary>
        /// <param name="v">Vector3 to clamp</param>
        /// <returns>Clamped vector</returns>
        public Vector3 BoundVector(Vector3 v) => new Vector3(Mathf.Clamp(v.x, this.xMin, this.xMax), 0f, Mathf.Clamp(v.z, this.zMin, this.zMax));
        #endregion
    }
}
