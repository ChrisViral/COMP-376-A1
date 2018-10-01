using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.UI
{
    /// <summary>
    /// Plays a sound on button click
    /// </summary>
    [RequireComponent(typeof(Button), typeof(AudioSource))]
    public class ButtonSound : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private AudioClip clip;
        [SerializeField]
        private float volume;

        //Private fields
        private AudioSource source;
        #endregion

        #region Methods
        /// <summary>
        /// Fires on button click and plays the given sound
        /// </summary>
        private void OnClick() => this.source.PlayOneShot(this.clip, this.volume);
        #endregion

        #region Functions
        private void Awake()
        {
            //Get AudioSource
            this.source = this.gameObject.GetComponent<AudioSource>();
            //Add event to button
            this.gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }
        #endregion
    }
}