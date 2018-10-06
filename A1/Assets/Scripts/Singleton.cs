using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Immortal singleton base class
    /// </summary>
    /// <typeparam name="T">MonoBehaviour type</typeparam>
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : LoggingBehaviour where T : MonoBehaviour
    {
        #region Instance
        /// <summary>
        /// Single immortal instance of this object
        /// </summary>
        public static T Instance { get; private set; }
        #endregion

        #region Functions
        /// <summary>
        /// Awake() function
        /// </summary>
        protected override void OnAwake()
        {
            //Check for an existing instance
            if (Instance)
            {
                Destroy(this.gameObject);
                return;
            }

            //If none exist, create it
            Instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion
    }
}
