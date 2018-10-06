using System;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Base class MonoBehaviour with logging method stubs
    /// </summary>
    public abstract class LoggingBehaviour : MonoBehaviour
    {
        #region Fields
        //private fields
        protected string className;
        #endregion

        #region Methods
        /// <summary>
        /// Logs an object message
        /// </summary>
        /// <param name="message">Message to log</param>
        protected void Log(object message) => Debug.Log($"[{this.className}]: {message}", this);

        /// <summary>
        /// Logs a given error message
        /// </summary>
        /// <param name="message">Message to log</param>
        protected void LogError(object message) => Debug.LogError($"[{this.className}]: {message}", this);

        /// <summary>
        /// Logs an exception with the given message
        /// </summary>
        /// <param name="e">Exception to log</param>
        /// <param name="message">Message to log</param>
        protected void LogException(Exception e, object message) => Debug.Log($"<color=red>[{this.className}]: {message}\n{e.GetType().Name}: {e.Message}\nStack trace: {e.StackTrace}</color>", this);

        /// <summary>
        /// This is called from within Awake, you should override this instead of writing an Awake() method
        /// </summary>
        protected virtual void OnAwake() { }
        #endregion

        #region Functions
        private void Awake()
        {
            //Get the class name
            this.className = GetType().Name;

            //Then call children OnAwake()
            OnAwake();
        }
        #endregion
    }
}
