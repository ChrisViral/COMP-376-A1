using System.Collections.Generic;
using SpaceShooter.Waves;
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
        private Text scoreLabel, restartLabel, gameoverLabel;
        [SerializeField]
        private GameObject asteroids;
        [SerializeField]
        private GameObject[] enemies;
        [SerializeField]
        private GameObject powerup, boss;
        [SerializeField]
        private Vector3 spawn, bossSpawn;
        [SerializeField]
        private int waves;

        //Private fields
        private int score;
        private bool gameEnded, bossFight;
        private WaveController asteroidController, enemyController;
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
        /// Ends the game cycle
        /// </summary>
        public void EndGame(bool won = false)
        {
            //End game
            this.gameEnded = true;
            this.asteroidController.StopWave();
            this.enemyController.StopWave();

            //Activate endgame text labels
            this.gameoverLabel.gameObject.SetActive(true);
            this.restartLabel.text += "\nPress R to restart...";

            if (won)
            {
                this.gameoverLabel.text = "Congratulations!";
            }
        }

        /// <summary>
        /// Starts a random Enemy wave controller
        /// </summary>
        private void StartRandomController()
        {
            if (this.waves-- > 0)
            {
                this.enemyController = Instantiate(this.enemies[Random.Range(0, this.enemies.Length)]).GetComponent<WaveController>();
                this.enemyController.StartWave();
            }
            else { StartCoroutine(StartBossFight()); }
        }

        private IEnumerator<YieldInstruction> StartBossFight()
        {
            this.bossFight = true;
            this.asteroidController.StopWave();
            Destroy(this.asteroidController.gameObject);
            yield return new WaitForSeconds(5f);

            Instantiate(this.boss, this.bossSpawn, Quaternion.identity);
        }

        /// <summary>
        /// Full enemy wave destroyed event
        /// </summary>
        public void WaveDestroyed()
        {
            this.score *= 2;
            Instantiate(this.powerup, this.spawn, Quaternion.identity);
        }
        #endregion

        #region Functions
        private void Start()
        {
            this.asteroidController = Instantiate(this.asteroids).GetComponent<AsteroidWaveController>();
            this.asteroidController.StartWave();
            StartRandomController();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameLogic.LoadScene(GameScenes.MENU);
                return;
            }

            if (this.gameEnded)
            {
                //If game ended, check for restart keypress
                if (Input.GetKeyDown(KeyCode.R))
                {
                    GameLogic.LoadScene(GameScenes.GAME);
                }
            }
            else if (!this.bossFight && !this.enemyController.IsRunning)
            {
                    StartRandomController();
            }
        }
        #endregion
    }
}