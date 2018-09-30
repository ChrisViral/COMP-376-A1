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
        [SerializeField]
        private int points;
        [SerializeField]
        private GameObject explosion;
        #endregion

        #region Function
        private void OnTriggerEnter(Collider other)
        {
            //Make sure the colliding object is not a game boundary
            if (!other.CompareTag("Boundary"))
            {
                //Figure out the object's tag
                switch (other.tag)
                {
                    //If a projectile, destroy both objects and trigger explosion particles
                    case "Projectile":
                        Destroy(this.gameObject);
                        Destroy(other.gameObject);
                        Instantiate(this.explosion, this.transform.position, Quaternion.identity);
                        GameLogic.Instance.UpdateScore(this.points);
                        break;
                    
                    //If player, kill player
                    case "Player":
                        other.GetComponent<Player>().Die();
                        break;

                    //If enemy, and current object a hazard, kill enemy
                    case "Enemy":
                        if (this.gameObject.CompareTag("Hazard"))
                        {
                            //Enemy.Die();
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
