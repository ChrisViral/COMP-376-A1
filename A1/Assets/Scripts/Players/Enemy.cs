using System.Collections;
using UnityEngine;

namespace SpaceShooter.Players
{
    /// <summary>
    /// Enemy ship
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(AudioSource))]
    public class Enemy : Ship
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Shooting delay")]
        private float minDelay;
        [SerializeField]
        private float maxDelay;
        #endregion

        #region Functions
        private IEnumerator Start()
        {
            //If the Enemy is allowed to shoot
            if (this.canShoot)
            {
                //Wait in the given delay range then start shooting
                this.canShoot = false;
                yield return new WaitForSeconds(Random.Range(this.minDelay, this.maxDelay));
                this.canShoot = true;
            }
        }
        #endregion
    }
}