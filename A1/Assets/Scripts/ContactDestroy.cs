using SpaceShooter.Waves;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Contact destructor object
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ContactDestroy : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private int points;
        [SerializeField]
        private GameObject explosion;
        #endregion

        #region Properties
        /// <summary>
        /// The WaveListener this object is associated to
        /// </summary>
        public WaveListener Listener { get; internal set; }

        /// <summary>
        /// If this object has already exploded or not
        /// </summary>
        public bool IsExploded { get; private set; }
        #endregion

        #region Methods
        public void Explode()
        {
            if (!this.IsExploded)
            {
                Destroy(this.gameObject);
                if (this.explosion != null) { Instantiate(this.explosion, this.transform.position, Quaternion.identity); }
                this.IsExploded = true;
            }
        }
        #endregion

        #region Function
        private void OnTriggerEnter(Collider other)
        {
            //Do not collide if exploded
            if (this.IsExploded) { return; }
            
            //Figure out the object's tag
            switch (other.tag)
            {
                //If a projectile, destroy both objects and trigger explosion particles
                case "Projectile":
                    Explode();
                    Destroy(other.gameObject);
                    GameLogic.CurrentGame.UpdateScore(this.points);
                    if (this.Listener != null) { this.Listener.OnKilled(); }
                    break;
                    
                //If player, kill player
                case "Player":
                    if (!other.GetComponent<Player>().Die()) { Explode(); }
                    break;
            }
        }

        private void OnDestroy()
        {
            //Let the listener know the object has been destroyed if any is present
            if (this.Listener != null) { this.Listener.OnDestroyed(); }
        }
        #endregion
    }
}
