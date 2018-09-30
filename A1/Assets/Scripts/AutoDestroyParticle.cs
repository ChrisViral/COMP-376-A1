using UnityEngine;

namespace A1
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestroyParticle : MonoBehaviour
    {
        private void Start() => Destroy(this.gameObject, this.gameObject.GetComponent<ParticleSystem>().main.duration);
    }
}
