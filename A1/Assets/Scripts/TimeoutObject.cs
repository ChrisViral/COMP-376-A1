using UnityEngine;

namespace A1
{
    public class TimeoutObject : MonoBehaviour
    {
        [SerializeField]
        private float time;

        private void Start() => Destroy(this.gameObject, this.time);
    }
}
