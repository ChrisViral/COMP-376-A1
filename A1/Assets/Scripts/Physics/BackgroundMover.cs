using UnityEngine;

namespace SpaceShooter.Physics
{
    /// <summary>
    /// Moves the background in specific ways
    /// </summary>
    [DisallowMultipleComponent]
    public class BackgroundMover : PhysicsObject
    {
        /// <summary>
        /// Background movement mode
        /// </summary>
        public enum MovementMode
        {
            ACCELERATE,
            APPROACH
        }

        #region Fields
        //Inspector fields
        [SerializeField]
        private float acceleration, accelerationDuration, approachSpeed;

        //Private fields
        private MovementMode mode;
        private float remainingTime, currentSpeed;
        #endregion

        #region Properties
        private bool active;
        /// <summary>
        /// If this BackgroundMover is currently active
        /// </summary>
        public bool Active
        {
            get { return this.active; }
            set
            {
                this.active = value;
                if (!this.active)
                {
                    this.remainingTime = 0f;
                    this.currentSpeed = 0;
                    this.rigidbody.velocity = Vector3.zero;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts a background movement
        /// </summary>
        /// <param name="m"></param>
        public void StartMovement(MovementMode m)
        {
            //Only start if inactive
            if (!this.Active)
            {
                //Set mode and active
                this.Active = true;
                this.mode = m;

                //Set time or speed depending on mode
                switch (m)
                {
                    case MovementMode.ACCELERATE:
                        this.remainingTime = this.accelerationDuration;
                        break;

                    case MovementMode.APPROACH:
                        this.currentSpeed = this.approachSpeed;
                        this.rigidbody.velocity = Vector3.back * this.approachSpeed;
                        break;
                }
            }
        }
        #endregion

        #region Functions
        private void FixedUpdate()
        {
            //Only update if active
            if (this.Active)
            {
                switch (this.mode)
                {
                    case MovementMode.ACCELERATE:
                        //Slowly accelerate over time
                        this.remainingTime -= Time.fixedDeltaTime;
                        if (this.remainingTime > 0)
                        {
                            this.rigidbody.AddForce(Vector3.back * this.acceleration, ForceMode.Acceleration);
                        }
                        else { this.Active = false; }
                        break;

                    case MovementMode.APPROACH:
                        //Gradually reduce speed to zero
                        this.currentSpeed -= Time.fixedDeltaTime * this.acceleration;
                        if (this.currentSpeed > 0)
                        {
                            this.rigidbody.velocity = Vector3.back * this.currentSpeed;
                        }
                        else { this.Active = false; } 
                        break;
                }
            }
        }
        #endregion
    }
}