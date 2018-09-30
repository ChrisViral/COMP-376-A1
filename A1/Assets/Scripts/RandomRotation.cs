using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Gives an object a random rotation upon load
    /// </summary>
    public class RandomRotation : PhysicsObject
    {
        #region Fields
        [SerializeField]
        private float maxRotation;
        #endregion

        #region Functions
        //Give the Rigidbody a random angular velocity
        private void Start() => this.rigidbody.angularVelocity = Random.insideUnitSphere * this.maxRotation;
        #endregion
    }
}
