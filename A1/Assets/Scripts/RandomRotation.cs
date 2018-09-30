using System.Runtime.CompilerServices;
using UnityEngine;

namespace A1
{
    public class RandomRotation : PhysicsObject
    {
        [SerializeField]
        private float maxRotation;

        private void Start() => this.rigidbody.angularVelocity = Random.insideUnitSphere * this.maxRotation;
    }
}
