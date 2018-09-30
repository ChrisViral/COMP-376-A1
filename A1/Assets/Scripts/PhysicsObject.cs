using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Physical object abstract class
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class PhysicsObject : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// This object's Rigidbody component
        /// </summary>
        protected new Rigidbody rigidbody;
        #endregion

        #region Functions
        ///Get Rigidbody from components
        protected void Awake() => this.rigidbody = this.gameObject.GetComponent<Rigidbody>();
        #endregion
    }
}
