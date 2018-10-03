using UnityEngine;

namespace SpaceShooter.Utils
{
    /// <summary>
    /// Bolt
    /// </summary>
    [DisallowMultipleComponent]
    public class Bolt : MonoBehaviour
    {
        #region Properties  
        /// <summary>
        /// State of the bolt, has not been destroyed if true
        /// Fetching this value will destroy the bolt and set this to false
        /// </summary>
        private bool active = true;
        public bool Active
        {
            get
            {
                if (this.active)
                {
                    this.active = false;
                    Destroy(this.gameObject);
                    return true;
                }
                return false;
            }
        }
        #endregion
    }
}
