using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Audio object playing a single clip base class
    /// </summary>
    [RequireComponent(typeof(AudioSource)), AddComponentMenu("Audio/Audio Object")]
    public class AudioObject : LoggingBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        protected AudioClip clip;
        [SerializeField]
        protected float volume;

        //Private fields
        protected AudioSource source;
        #endregion

        #region Virtual methods
        /// <summary>
        /// Plays this object's clip at the given volume
        /// </summary>
        public virtual void PlayClip() => this.source.PlayOneShot(this.clip, this.volume);
        #endregion

        #region Functions
        protected override void OnAwake()
        {
            //Call base method
            base.OnAwake();

            //Gets the AudioSource
            this.source = GetComponent<AudioSource>();
        }
        #endregion
    }
}
