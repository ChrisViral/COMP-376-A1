using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Gives an object a constant forward motion upon load
    /// </summary>
    public class StraightMovement : PhysicsObject
    {
        #region Fields
        [SerializeField]
        private float speed;
        #endregion

        #region Functions
        //Set requested speed
        private void Start() => this.rigidbody.velocity = this.transform.forward * this.speed;
        #endregion
    }
}
