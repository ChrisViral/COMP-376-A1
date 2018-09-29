using UnityEngine;

namespace A1
{
    public class TimeoutObject : MonoBehaviour
    {
        [SerializeField]
        private float life;
        private float timeout;

        private void Start() => this.timeout = Time.time + this.life;

        private void Update()
        {
            if (Time.time > this.timeout)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
