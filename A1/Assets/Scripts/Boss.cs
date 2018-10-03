using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Boss : Ship
    {
        [SerializeField]
        private GameObject vulnerability;
        [SerializeField]
        private Transform[] vulnerabilityLocations, guns;
        [SerializeField]
        private GameObject core;
        [SerializeField]
        private float coreTime, vulnerabilityDelay;
        [SerializeField]
        private float arrivalTime, arrivalSpeed, movementPeriod;
        [SerializeField]
        private int explosionCount, score;

        private float arrival, elapsedTime;
        private bool arrived;

        public int ActiveVulnerabilities { get; private set; }

        public void DestroyVulnerability()
        {
            if (this.core.activeSelf)
            {
                Die();
            }
            else if (--this.ActiveVulnerabilities == 0)
            {
                ExposeCore();
            }
        }

        public void ExposeCore()
        {
            this.core.SetActive(true);
            StartCoroutine(ShutoffCore());
        }

        private IEnumerator<YieldInstruction> ShutoffCore()
        {
            yield return new WaitForSeconds(this.coreTime);
            this.core.SetActive(false);

            yield return new WaitForSeconds(this.vulnerabilityDelay);
            AddVulnerabilities();
        }

        private void AddVulnerabilities()
        {
            this.ActiveVulnerabilities = this.vulnerabilityLocations.Length;
            foreach (Transform t in this.vulnerabilityLocations)
            {
                GameObject go = Instantiate(this.vulnerability, Vector3.zero, Quaternion.identity);
                go.transform.SetParent(t, false);
                go.GetComponent<Vulnerability>().boss = this;
            }
        }

        public override bool Die()
        {
            for (int i = 0; i < this.explosionCount; i++)
            {
                Instantiate(this.explosion, this.transform.position + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-2f, 2f)), Quaternion.identity);
            }

            Destroy(this.gameObject);
            return true;
        }

        protected override void Fire()
        {
            Instantiate(this.bolt, this.guns[Random.Range(0, this.guns.Length)].position, Quaternion.identity);
            this.source.PlayOneShot(this.boltSound, this.shotVolume);
        }

        protected override void OnFixedUpdate()
        {
            if (!this.arrived)
            {
                this.arrival -= Time.fixedDeltaTime;
                if (this.arrival > 0)
                {
                    this.rigidbody.velocity = new Vector3(0f, 0f, -Mathf.Lerp(0f, this.arrivalSpeed, this.arrival / this.arrivalTime));
                }
                else
                {
                    this.arrival = 0f;
                    this.arrived = true;
                }
            }
            else
            {
                float t = this.elapsedTime / this.movementPeriod;
                this.rigidbody.velocity = new Vector3(Mathf.Cos(t), 0f, Mathf.Cos(2 * t)) * this.speed;
                this.elapsedTime += Time.fixedDeltaTime;
            }
        }

        private IEnumerator Start()
        {
            this.canShoot = false;
            this.arrival = this.arrivalTime;
            yield return new WaitForSeconds(this.vulnerabilityDelay + this.arrivalTime);
            AddVulnerabilities();
            this.canShoot = true;
        }

        private void OnDestroy()
        {
            Game game = GameLogic.CurrentGame;
            game.UpdateScore(this.score);
            game.EndGame(true);
        }
    }
}