using UnityEngine;

namespace A1
{
    [RequireComponent(typeof(Collider))]
    public class ContactDestroy : MonoBehaviour
    {
        [SerializeField]
        private int points;
        [SerializeField]
        private GameObject explosion;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Boundary"))
            {
                switch (other.tag)
                {
                    case "Projectile":
                        Destroy(this.gameObject);
                        Instantiate(this.explosion, this.transform.position, Quaternion.identity);
                        GameLogic.Instance.UpdateScore(this.points);
                        break;

                    case "Player":
                        other.GetComponent<Player>().Die();
                        break;

                    case "Enemy":
                        if (this.gameObject.CompareTag("Hazard"))
                        {
                            //Enemy.Die();
                        }
                        break;
                }
            }
        }
    }
}
