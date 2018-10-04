using UnityEngine;

namespace SpaceShooter.Physics
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
        private void Awake()
        {
            //Get Rigidbody from components
            this.rigidbody = GetComponent<Rigidbody>();
            OnAwake();
        }
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Is called alongside the PhysicsObject Awake() function, use to access the Awake function
        /// </summary>
        protected virtual void OnAwake() { }
        #endregion
    }
}
