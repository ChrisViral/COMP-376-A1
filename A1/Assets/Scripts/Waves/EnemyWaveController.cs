using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Waves
{
    /// <summary>
    /// Sinusoidal movement enemy wave generator
    /// </summary>
    public class EnemyWaveController : WaveController
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private GameObject enemy;
        [SerializeField]
        private float interval;
        [SerializeField]
        private int count;
        [SerializeField]
        private WaveListener listener;
        #endregion
        
        #region Methods
        /// <summary>
        /// Spawns the Sinusoidal enemy wave
        /// </summary>
        protected override IEnumerator<YieldInstruction> SpawnWave()
        {
            //Setup listener
            this.listener.Active = true;
            this.listener.Count = this.count;

            //First wave delay
            yield return new WaitForSeconds(this.delay);

            //Spawn first enemy
            this.listener.AttachListener(Instantiate(this.enemy, this.spawn, Quaternion.identity));

            //Spawn wave
            for (int i = 1; i < this.count; i++)
            {
                //Wait a before spawning next enemy
                yield return new WaitForSeconds(this.interval);

                //Spawn enemy
                this.listener.AttachListener(Instantiate(this.enemy, this.spawn, Quaternion.identity));
            }
        }
        #endregion
    }
}