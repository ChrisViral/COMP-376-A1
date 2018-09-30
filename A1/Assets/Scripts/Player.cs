using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Player object
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Player : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float speed, tilt, fireRate;
        [SerializeField]
        private Transform gun;
        [SerializeField]
        private GameObject bolt, explosion;
        [SerializeField]
        private AudioClip boltSound;
        [SerializeField]
        private float shotVolume;
        [SerializeField]
        private Bounds gameLimits;
        
        //Private fields
        private AudioSource source;
        private float nextFire;
        #endregion

        #region Methods
        /// <summary>
        /// Kills the player
        /// </summary>
        public void Die()
        {
            //Destroy object and create explosion
            Destroy(this.gameObject);
            Instantiate(this.explosion, this.transform.position, Quaternion.identity);

            //Notify for the game to end
            GameLogic.Instance.EndGame();
        }
        #endregion

        #region Functions
        private new void Awake()
        {
            base.Awake();
            this.source = this.gameObject.GetComponent<AudioSource>();
        }

        private void Update()
        {
            //If fire is pressed and enough time has elapsed since last fire, spawn a new shot
            if (Input.GetButton("Fire1") && Time.time > this.nextFire)
            {
                this.nextFire = Time.time + this.fireRate;
                Instantiate(this.bolt, this.gun.position, Quaternion.identity);
                this.source.PlayOneShot(this.boltSound, this.shotVolume);
            }
        }

        private void FixedUpdate()
        {
            //Movement speed
            this.rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * this.speed, 0f, Input.GetAxis("Vertical") * this.speed);
            //Limit to game bounds
            this.rigidbody.position = this.gameLimits.BoundVector(this.rigidbody.position);
            //Side tilt
            this.rigidbody.rotation = Quaternion.Euler(0f, 0f, this.rigidbody.velocity.x * -this.tilt);
        }
        #endregion
    }
}
