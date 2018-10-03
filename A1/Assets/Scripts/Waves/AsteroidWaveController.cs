using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Waves
{
    /// <summary>
    /// Asteroid generation wave controller
    /// </summary>
    public class AsteroidWaveController : WaveController
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private GameObject[] asteroids;
        [SerializeField]
        private int max;
        [SerializeField]
        private Vector2 spawnTimeRange;
        #endregion

        #region Methods
        /// <summary>
        /// Asteroid wave generator
        /// </summary>
        protected override IEnumerator<YieldInstruction> SpawnWave()
        {
            //First wave delay
            yield return new WaitForSeconds(this.delay);

            //Spawn loop
            while (true)
            {
                //Spawn wave
                for (int i = 0, count = Random.Range(1, this.max + 1); i < count; i++)
                {
                    Instantiate(this.asteroids[Random.Range(0, this.asteroids.Length)], new Vector3(Random.Range(this.spawn.x, this.spawn.y), 0f, this.spawn.z), Quaternion.identity);
                }

                //Wait a random amount of time before spawning next wave
                yield return new WaitForSeconds(Random.Range(this.spawnTimeRange.x, this.spawnTimeRange.y));
            }
        }
        #endregion

    }
}