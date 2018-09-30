using System;
using UnityEngine;

namespace A1
{
    public class Player : PhysicsObject
    {
        [SerializeField]
        private float speed, tilt, fireRate;
        [SerializeField]
        private Transform gun;
        [SerializeField]
        private GameObject bolt, explosion;
        [SerializeField]
        private Bounds gameLimits;
        
        private float nextFire;

        public void Die()
        {
            Destroy(this.gameObject);
            Instantiate(this.explosion, this.transform.position, Quaternion.identity);
            GameLogic.Instance.EndGame();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1") && Time.time > this.nextFire)
            {
                this.nextFire = Time.time + this.fireRate;
                Instantiate(this.bolt, this.gun.position, Quaternion.identity);
            }
        }

        private void FixedUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            this.rigidbody.velocity = new Vector3(horizontal * this.speed, 0f, vertical * this.speed);
            this.rigidbody.position = this.gameLimits.BoundVector(this.rigidbody.position);
            this.rigidbody.rotation = Quaternion.Euler(0f, 0f, this.rigidbody.velocity.x * -this.tilt);
        }
    }
}
