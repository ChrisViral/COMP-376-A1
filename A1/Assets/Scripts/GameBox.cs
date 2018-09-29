using UnityEngine;

namespace A1
{
    public class GameBox : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}
