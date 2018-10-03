using SpaceShooter.Physics;
using UnityEngine;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Ship base class
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public abstract class Ship : PhysicsObject
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Movement")]
        protected float speed;
        [SerializeField]
        protected float tilt;
        [SerializeField]
        protected GameObject explosion;
        [SerializeField, Header("Fire")]
        protected GameObject bolt;
        [SerializeField]
        protected AudioClip boltSound;
        [SerializeField]
        protected float shotVolume, fireRate;
        [SerializeField]
        protected Transform gun;
        public bool canShoot = true;
        
        //Private fields
        protected AudioSource source;
        protected float nextFire;
        #endregion

        #region Methods
        /// <summary>
        /// Kills the player
        /// </summary>
        public virtual bool Die()
        {
            //Destroy object and create explosion
            Destroy(this.gameObject);
            Instantiate(this.explosion, this.transform.position, Quaternion.identity);
            return true;
        }

        /// <summary>
        /// Fires the ship's weapon if possible
        /// </summary>
        /// <returns>True if the weapon has fired, false otherwise</returns>
        public bool FireGun()
        {
            if (this.canShoot && Time.time > this.nextFire)
            {
                //Fire the weapon
                this.nextFire = Time.time + this.fireRate;
                Fire();
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Fires the ship's weapon
        /// </summary>
        protected virtual void Fire()
        {
            Instantiate(this.bolt, this.gun.position, Quaternion.identity);
            this.source.PlayOneShot(this.boltSound, this.shotVolume);
        }

        /// <summary>
        /// OnAwake function
        /// </summary>
        protected override void OnAwake()
        {
            //Get the audio source
            this.source = this.gameObject.GetComponent<AudioSource>();
        }
        #endregion

        #region Functions
        private void FixedUpdate()
        {
            //Call children object FixedUpdate
            OnFixedUpdate();

            //Side tilt
            this.rigidbody.rotation = Quaternion.Euler(0f, 0f, this.rigidbody.velocity.x * -this.tilt);
        }

        //Basic update loop
        private void Update() => OnUpdate();
        #endregion

        #region Virtual Methods
        /// <summary>
        /// Is called alongside the Ship FixedUpdate() function, use to access the FixedUpdate function
        /// </summary>
        protected virtual void OnFixedUpdate() { }

        /// <summary>
        /// Is called alongside with the Ship Update function, allows overriding base functionality
        /// </summary>
        protected virtual void OnUpdate() => FireGun();
        #endregion
    }
}
