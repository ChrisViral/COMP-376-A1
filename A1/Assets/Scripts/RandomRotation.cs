using UnityEngine;

namespace A1
{
    public class RandomRotation : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;
        [SerializeField]
        private float maxRotation;

        private void Start() => this.rigidbody.angularVelocity = Random.insideUnitSphere * this.maxRotation;
    }
}
