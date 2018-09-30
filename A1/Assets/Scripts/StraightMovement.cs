using UnityEngine;

namespace A1
{
    public class StraightMovement : PhysicsObject
    {
        [SerializeField]
        private float speed;

        private void Start() => this.rigidbody.velocity = this.transform.forward * this.speed;
    }
}
