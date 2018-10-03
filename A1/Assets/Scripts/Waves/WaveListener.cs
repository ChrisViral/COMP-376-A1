using UnityEngine;

namespace SpaceShooter.Waves
{
    [RequireComponent(typeof(WaveController))]
    public class WaveListener : MonoBehaviour
    {
        #region Fields
        //Private fields
        private int killed, destroyed;
        #endregion

        #region Properties
        /// <summary>
        /// Amount of objects this listener watches
        /// </summary>
        public int Count { get; internal set; }

        public bool Active { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Attaches this WaveListener instance to the given GameObject if possible, then returns it
        /// </summary>
        /// <param name="o">GameObject to attach to</param>
        /// <returns>The passed GameObject instance</returns>
        public GameObject AttachListener(GameObject o)
        {
            ContactDestroy destroyer = o.GetComponent<ContactDestroy>();
            if (destroyer != null) { destroyer.Listener = this; }
            return o;
        }

        /// <summary>
        /// Call this to indicate a part of the wave has been killed
        /// </summary>
        public void OnKilled() => this.killed++;

        /// <summary>
        /// Call this to indicate a part of the wave has been destroyed
        /// </summary>
        public void OnDestroyed() => this.destroyed++;
        #endregion

        #region Functions
        private void Update()
        {
            if (!this.Active) { return; }

            //When all objects have been destroyed
            if (this.destroyed == this.Count)
            {
                //If they have also all been killed, send a completion message
                if (this.killed == this.Count)
                {
                    this.Active = false;
                    GameLogic.CurrentGame.WaveDestroyed();
                }

                //Destroy only this script
                Destroy(this.gameObject);
            }
        }
        #endregion
    }
}
