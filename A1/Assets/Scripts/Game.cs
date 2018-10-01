using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    /// <summary>
    /// Gameplay flow controller
    /// </summary>
    [DisallowMultipleComponent]
    public class Game : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private Vector3 spawn;
        [SerializeField]
        private GameObject[] asteroids;
        [SerializeField]
        private Vector2 spawnTimeRange;
        [SerializeField]
        private int maxSpawn, startWait;
        [SerializeField]
        private Text scoreLabel, restartLabel, gameoverLabel;

        //Private fields
        private int score;
        private bool gameEnded;
        private Coroutine spawner;  //Asteroid spawner coroutine
        #endregion

        #region Methods
        /// <summary>
        /// Updates the current player score by adding the specified amount of points
        /// </summary>
        /// <param name="added">Points to add</param>
        public void UpdateScore(int added)
        {
            this.score += added;
            this.scoreLabel.text = $"Score: {this.score}";
        }

        /// <summary>
        /// Asteroid spawning coroutine
        /// </summary>
        private IEnumerator<YieldInstruction> SpawnAsteroids()
        {
            //First wave delay
            yield return new WaitForSeconds(this.startWait);

            //Spawn loop
            while (true)
            {
                //Spawn wave
                for (int i = 0, count = Random.Range(1, this.maxSpawn + 1); i < count; i++)
                {
                    Instantiate(this.asteroids[Random.Range(0, this.asteroids.Length)], new Vector3(Random.Range(this.spawn.x, this.spawn.y), 0f, this.spawn.z), Quaternion.identity);
                }

                //Wait a random amount of time before spawning next wave
                yield return new WaitForSeconds(Random.Range(this.spawnTimeRange.x, this.spawnTimeRange.y));
            }
        }

        /// <summary>
        /// Ends the game cycle
        /// </summary>
        public void EndGame()
        {
            //End game
            this.gameEnded = true;
            StopCoroutine(this.spawner);

            //Activate endgame text labels
            this.gameoverLabel.gameObject.SetActive(true);
            this.restartLabel.gameObject.SetActive(true);
        }
        #endregion

        #region Functions
        private void Start()
        {
            this.spawner = StartCoroutine(SpawnAsteroids());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameLogic.LoadScene(GameScenes.MENU);
                return;
            }

            //If game ended, check for restart keypress
            if (this.gameEnded)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    GameLogic.LoadScene(GameScenes.GAME);
                }
            }
        }
        #endregion
    }
}