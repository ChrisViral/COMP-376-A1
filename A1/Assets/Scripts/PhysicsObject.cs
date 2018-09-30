using UnityEngine;

namespace A1
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsObject : MonoBehaviour
    {
        protected new Rigidbody rigidbody;

        private void Awake() => this.rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }
}
