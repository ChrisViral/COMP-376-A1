using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(AudioSource))]
    public class ProjectileDestroyer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip sound;
        [SerializeField]
        private float volume;

        private AudioSource source;

        private void Awake() => this.source = this.gameObject.GetComponent<AudioSource>();

        private void OnTriggerEnter(Collider other)
        {
            //Figure out the object's tag
            switch (other.tag)
            {
                //If a projectile, destroy both objects and trigger explosion particles
                case "Projectile":
                    Destroy(other.gameObject);
                    this.source.PlayOneShot(this.sound, this.volume);
                    break;
                    
                //If player, kill player
                case "Player":
                    other.GetComponent<Player>().Die();
                    break;
            }
        }
    }
}