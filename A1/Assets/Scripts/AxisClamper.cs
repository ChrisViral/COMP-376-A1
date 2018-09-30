using UnityEngine;

namespace A1
{
    public class AxisClamper : PhysicsObject
    {
        [SerializeField]
        private float value;

        private void FixedUpdate()
        {
            Vector3 pos = this.rigidbody.position;
            pos.y = this.value;
            this.rigidbody.position = pos;
        }
    }
}
