using UnityEngine;

namespace A1
{
    public class AutoDestroyParticle : MonoBehaviour
    {
        private void Start() => Destroy(this.gameObject, this.gameObject.GetComponent<ParticleSystem>().main.duration);
    }
}
