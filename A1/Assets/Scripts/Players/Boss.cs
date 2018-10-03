using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Physics;
using SpaceShooter.Scenes;
using SpaceShooter.UI;
using UnityEngine;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Boss ship
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(FigureEightMovement))]
    public class Boss : Ship
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Vulnerabilities")]
        private GameObject vulnerability;
        [SerializeField]
        private GameObject core;
        [SerializeField]
        private float coreTime, vulnerabilityDelay;
        [SerializeField]
        private AudioClip vulnerabilitySound;
        [SerializeField]
        private float vulnerabilityVolume;
        [SerializeField, Tooltip("Vulnerabilities location")]
        private Transform[] vulnerabilities;
        [SerializeField, Header("Boss fight"), Tooltip("Guns location")]
        private Transform[] guns;
        [SerializeField, Tooltip("Time taken for the boss to come into the screen")]
        private float arrivalTime;
        [SerializeField]
        private int explosionCount, score;
        [SerializeField]
        private int maxHealth;

        //Private fields
        private int hp;
        private float maxHP;
        private Progressbar healthbar;
        private float arrival, elapsedTime;
        private bool arrived;
        #endregion
        
        #region Properties
        /// <summary>
        /// Number of active vulnerabilities on this boss ship
        /// </summary>
        public int ActiveVulnerabilities { get; private set; }

        /// <summary>
        /// If the Core of this Boss ship is visible
        /// </summary>
        public bool CoreVisible
        {
            get { return this.core.activeSelf; }
            set { this.core.SetActive(value); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds new vulnerabilities to the boss ship
        /// </summary>
        private void AddVulnerabilities()
        {
            //Set max amount of vulnerabilities
            this.ActiveVulnerabilities = this.vulnerabilities.Length;

            //Loop through all transforms and add a vulnerability
            foreach (Transform t in this.vulnerabilities)
            {
                //Instantiate the Vulnerability as a child of the anchor point
                Instantiate(this.vulnerability, t, false);
            }
        }

        /// <summary>
        /// Vulnerability hit event
        /// </summary>
        public void HitVulnerability() => this.source.PlayOneShot(this.vulnerabilitySound, this.vulnerabilityVolume);

        /// <summary>
        /// Vulnerability destroyed event
        /// </summary>
        public void DestroyVulnerability()
        {
            //If no more vulnerabilities are left, expose the core
            if (!this.core.activeSelf && --this.ActiveVulnerabilities == 0)
            {
                ExposeCore();
            }
        }

        /// <summary>
        /// Exposes the Boss' core
        /// </summary>
        public void ExposeCore()
        {
            //Set the core active, then start the shutoff timer
            this.CoreVisible = true;
            StartCoroutine(ShutoffCore());
        }

        /// <summary>
        /// Core shutoff coroutine
        /// </summary>
        private IEnumerator<YieldInstruction> ShutoffCore()
        {
            //Wait the given delay then turn core off
            yield return new WaitForSeconds(this.coreTime);
            this.CoreVisible = false;

            //Wait the given delay, then add new vulnerabilities
            yield return new WaitForSeconds(this.vulnerabilityDelay);
            AddVulnerabilities();
        }

        /// <summary>
        /// Core hit event
        /// </summary>
        public void HitCore()
        {
            this.source.PlayOneShot(this.vulnerabilitySound, this.vulnerabilityVolume);
            this.healthbar.Progress = --this.hp / this.maxHP;

            //If out of HP, kill the Boss
            if (this.hp == 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Die event
        /// </summary>
        /// <returns>Always true</returns>
        public override bool Die()
        {
            //Spawn the given number of explosions, randomly
            for (int i = 0; i < this.explosionCount; i++)
            {
                Instantiate(this.explosion, this.transform.position + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-2f, 2f)), Quaternion.identity);
            }

            //Clean out the remaining stuff
            this.healthbar.GetComponent<FadeGraphics>().Fade(true);
            Game game = GameLogic.CurrentGame;
            game.UpdateScore(this.score);
            game.EndGame(true);
            Destroy(this.gameObject);
            return true;
        }

        /// <summary>
        /// Fire event
        /// </summary>
        protected override void Fire()
        {
            //Fire at a random gun location
            Instantiate(this.bolt, this.guns[Random.Range(0, this.guns.Length)].position, Quaternion.identity);
            this.source.PlayOneShot(this.boltSound, this.shotVolume);
        }

        /// <summary>
        /// FixedUpdate function
        /// </summary>
        protected override void OnFixedUpdate()
        {
            //Until the ship has arrived, move it
            if (!this.arrived)
            {
                //Get time until arrival
                this.arrival -= Time.fixedDeltaTime;
                
                if (this.arrival > 0)
                {
                    //Set speed until arrived
                    this.rigidbody.velocity = this.transform.forward * Mathf.Lerp(0f, this.speed, this.arrival / this.arrivalTime);
                }
                else
                {
                    //When arrived, start figure eight movement
                    this.gameObject.GetComponent<FigureEightMovement>().enabled = true;
                    this.arrived = true;
                }
            }
        }
        #endregion

        #region Functions
        private IEnumerator Start()
        {
            //Setup boss
            this.canShoot = false;
            this.healthbar = GameLogic.CurrentGame.bossProgressbar;
            this.maxHP = this.hp = this.maxHealth;

            //Setup arrival, then wait for arrival
            this.arrival = this.arrivalTime;
            this.rigidbody.velocity = this.transform.forward * this.speed;
            yield return new WaitForSeconds(this.vulnerabilityDelay + this.arrivalTime);

            //Add vulnerabilities and start shooting
            AddVulnerabilities();
            this.canShoot = true;
        }
        #endregion
    }
}