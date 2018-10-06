using SpaceShooter.Utils;
using UnityEngine;

namespace SpaceShooter.Physics
{
    /// <summary>
    /// Physical object abstract class
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class PhysicsObject : PausableObject
    {
        #region Fields
        /// <summary>
        /// This object's Rigidbody component
        /// </summary>
        protected new Rigidbody rigidbody;
        #endregion

        #region Functions
        /// <summary>
        /// Awake() function
        /// </summary>
        protected override void OnAwake()
        {
            //Call base OnAwake method
            base.OnAwake();

            //Get Rigidbody from components
            this.rigidbody = GetComponent<Rigidbody>();
        }
        #endregion
    }
}
