﻿using UnityEngine;
using UnityEngine.UI;
using Bounds = SpaceShooter.Utils.Bounds;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Player object
    /// </summary>
    public class Player : Ship
    {
        #region Constants
        /// <summary>
        /// Max player level
        /// </summary>
        public const int MAX_LEVEL = 2;
        /// <summary>
        /// Life off alpha value
        /// </summary>
        private const float OFF = 0.25f;
        #endregion

        #region Fields
        //Inspector fields
        [SerializeField, Header("Player")]
        private Bounds gameLimits; 
        [SerializeField]
        private Shield shield;
        [SerializeField]
        private Graphic[] lives;
        [SerializeField, Header("Powerup")]
        private AudioClip powerupSound;
        [SerializeField]
        private float powerupVolume;
        #endregion

        #region Properties
        /// <summary>
        /// Current level of the player
        /// </summary>
        public int Level { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Increments the player's level by one, up to Level 2
        /// </summary>
        /// <returns>The new player's level</returns>
        public int IncrementLevel()
        {
            if (this.Level < 2)
            {
                this.source.PlayOneShot(this.powerupSound, this.powerupVolume);
                this.lives[++this.Level].CrossFadeAlpha(1f, 0f, true);
            }
            return this.Level;
        }

        /// <summary>
        /// Decrements the players level, down to -1 (dead)
        /// </summary>
        /// <returns>The new player's level</returns>
        public int DecrementLevel()
        {
            if (this.Level >= 0)
            {
                this.lives[this.Level--].CrossFadeAlpha(OFF, 0f, true);
            }
            return this.Level;
        }

        /// <summary>
        /// Kills the player
        /// </summary>
        public override bool Die()
        {
            Debug.Log($"Die called at level {this.Level}");
            //Make sure life is back to zero
            if (DecrementLevel() < 0)
            {
                //Call base method
                base.Die();
                //Notify for the game to end
                GameLogic.CurrentGame.EndGame();
                return true;
            }

            this.shield.TriggerShield();
            return false;
        }

        /// <summary>
        /// Fires the weapon, according to the player's current level
        /// </summary>
        protected override void Fire()
        {
            //Normal fire
            if (this.Level == 0) { base.Fire(); }
            else
            {
                //Level 1 fire
                Vector3 pos = this.gun.position;
                pos.x -= 0.25f;
                Instantiate(this.bolt, pos, Quaternion.identity);
                pos.x += 0.5f;
                Instantiate(this.bolt, pos, Quaternion.identity);

                //Level 2 fire
                if (this.Level == 2)
                {
                    pos.x += 0.25f;
                    Instantiate(this.bolt, pos, Quaternion.Euler(0f,  25f, 0f));
                    pos.x -= 1f;
                    Instantiate(this.bolt, pos, Quaternion.Euler(0f, -25f, 0f));
                }

                //Play fire sound
                this.source.PlayOneShot(this.boltSound, this.shotVolume);
            }
        }

        /// <summary>
        /// FixedUpdate function
        /// </summary>
        protected override void OnFixedUpdate()
        {
            //Movement speed
            this.rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * this.speed, 0f, Input.GetAxis("Vertical") * this.speed);
            //Limit to game bounds
            this.rigidbody.position = this.gameLimits.BoundVector(this.rigidbody.position);
        }

        /// <summary>
        /// Update function
        /// </summary>
        protected override void OnUpdate()
        {
            //If fire is pressed and enough time has elapsed since last fire, spawn a new shot
            if (Input.GetButton("Fire1"))
            {
                FireGun();
            }
        }
        #endregion

        #region Functions
        private void Start()
        {
            //Crossfade both secondary icons
            this.lives[1].CrossFadeAlpha(OFF, 0f, true);
            this.lives[2].CrossFadeAlpha(OFF, 0f, true);
        }

        private void OnTriggerEnter(Collider other)
        {
            //If the shield is inactive and hit by an enemy, damage player
            if (!this.shield.Active && other.CompareTag("Projectile_Enemy"))
            {
                Die();
            }
        }
        #endregion
    }
}