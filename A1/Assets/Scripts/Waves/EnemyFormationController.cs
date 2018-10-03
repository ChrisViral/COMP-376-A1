using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Waves
{
    /// <summary>
    /// V-Formation enemy wave generator
    /// </summary>
    public class EnemyFormationController : WaveController
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private GameObject enemy;
        [SerializeField]
        private float interval, spacing;
        [SerializeField]
        private int layers;
        [SerializeField]
        private WaveListener listener;
        #endregion
       
        #region Methods
        /// <summary>
        /// Spawns the V-Formation enemy wave
        /// </summary>
        protected override IEnumerator<YieldInstruction> SpawnWave()
        {
            //Create the Listener
            this.listener.Active = true;
            this.listener.Count = (this.layers * 2) - 1;

            //First wave delay
            yield return new WaitForSeconds(this.delay);

            //Spawn first enemy
            
            this.listener.AttachListener(Instantiate(this.enemy, this.spawn, Quaternion.identity));

            //Spawn remaining layers
            for (int i = 1; i < this.layers; i++)
            {
                //Wait between spawns
                yield return new WaitForSeconds(this.interval);

                //Spawn both enemies side by side
                float space = this.spacing * i;
                this.listener.AttachListener(Instantiate(this.enemy, new Vector3(this.spawn.x - space, this.spawn.y, this.spawn.z), Quaternion.identity));
                this.listener.AttachListener(Instantiate(this.enemy, new Vector3(this.spawn.x + space, this.spawn.y, this.spawn.z), Quaternion.identity));
            }
        }
        #endregion
    }
}