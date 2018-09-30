using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Particle auto destroyer
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestroyParticle : MonoBehaviour
    {
        #region Functions
        //Set object to be destroyed as soon as the ParticleSystem has played one full cycle
        private void Start() => Destroy(this.gameObject, this.gameObject.GetComponent<ParticleSystem>().main.duration);
        #endregion
    }
}
