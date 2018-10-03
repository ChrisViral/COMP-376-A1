using UnityEngine;

namespace SpaceShooter.Physics
{
    public class SinusoidalMovement : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float peakAcceleration, period;

        //Private fields
        private float spawnTime;
        #endregion

        #region Properties
        /// <summary>
        /// Elapsed time since this object's creation
        /// </summary>
        private float ElapsedTime => Time.fixedTime - this.spawnTime;
        #endregion

        #region Functions
        //Get spawn time
        private void Start() => this.spawnTime = Time.fixedTime;

        //Add an acceleration to the object over time
        private void FixedUpdate() => this.rigidbody.AddForce(new Vector3(Mathf.Cos(this.ElapsedTime / this.period) * this.peakAcceleration, 0f, 0f), ForceMode.Acceleration);
        #endregion
    }
}
