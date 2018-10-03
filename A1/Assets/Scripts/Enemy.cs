using System.Collections;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(AudioSource))]
    public class Enemy : Ship
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float delay;
        #endregion

        #region Functions
        //Wait a certain time before starting to shoot
        private IEnumerator Start()
        {
            if (this.canShoot)
            {
                this.canShoot = false;
                yield return new WaitForSeconds(this.delay);
                this.canShoot = true;
            }
        }
        #endregion
    }
}