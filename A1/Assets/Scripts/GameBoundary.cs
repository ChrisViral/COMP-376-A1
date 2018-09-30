using UnityEngine;

namespace A1
{
    [RequireComponent(typeof(Collider))]
    public class GameBoundary : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}
