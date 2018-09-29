using UnityEngine;

namespace A1
{
    public class ContactDestroy : MonoBehaviour
    {
        [SerializeField]
        private GameObject explosion;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Boundary"))
            {
                Destroy(this.gameObject);
                Instantiate(this.explosion, this.transform.position, Quaternion.identity);
                Player player = other.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.Die();
                }
                else
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
