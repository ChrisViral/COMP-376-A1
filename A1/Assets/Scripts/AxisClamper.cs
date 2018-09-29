using UnityEngine;

namespace A1
{
    public class AxisClamper : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rigidbody;
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
