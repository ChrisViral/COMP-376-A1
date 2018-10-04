using System.Collections.Generic;
using SpaceShooter.Physics;
using SpaceShooter.Players;
using SpaceShooter.UI;
using SpaceShooter.Waves;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Scenes
{
    /// <summary>
    /// Gameplay flow controller
    /// </summary>
    [DisallowMultipleComponent]
    public class Game : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField, Header("Gameplay")]
        private Player player;
        [SerializeField]
        private AccelerationMovement background;
        [SerializeField]
        private FadeGraphics fadeToBlack, bossFade;
        [SerializeField]
        private float endGameWait;
        [SerializeField, Header("UI")]
        private Text scoreLabel;
        [SerializeField]
        private Text restartLabel, gameoverLabel;
        [SerializeField, Header("Enemy waves")]
        private int waves;
        [SerializeField, Tooltip("Asteroid spawner")]
        private GameObject asteroids;
        [SerializeField, Tooltip("Enemy spawners")]
        private GameObject[] enemies;
        [SerializeField, Header("Powerup")]
        private GameObject powerup;
        [SerializeField]
        private Vector3 powerupSpawn;
        [SerializeField, Header("Boss fight")]
        private GameObject boss;
        [SerializeField]
        private Vector3 bossSpawn;
        [SerializeField]
        private float bossDelay;
        [SerializeField, Tooltip("Boss health bar")]
        internal Progressbar bossProgressbar;

        //Private fields
        private GameMode mode;
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

            //Start the transition Coroutine
            StartCoroutine(won ? WinTransition() : LoseTransition());
        }

        /// <summary>
        /// Winning endgame coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator<YieldInstruction> WinTransition()
        {
            //Wait for the endgame
            yield return new WaitForSeconds(this.endGameWait);

            //Update UI
            this.gameoverLabel.gameObject.SetActive(true);
            this.gameoverLabel.text = "Congratulations!";
            this.restartLabel.text += "\nPress R to restart...";

            //Fade out screen
            this.background.StartMovement(AccelerationMovement.MovementMode.ACCELERATE);
            this.player.Controllable = false;
            this.player.GetComponent<AccelerationMovement>().StartMovement(AccelerationMovement.MovementMode.ACCELERATE);
            this.fadeToBlack.Fade();
        }

        /// <summary>
        /// Losing endgame coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator<YieldInstruction> LoseTransition()
        {
            //Stop spawners
            this.asteroidController.StopWave();
            this.enemyController.StopWave();

            //Update UI and fade
            this.gameoverLabel.gameObject.SetActive(true);
            this.restartLabel.text += "\nPress R to restart...";
            this.fadeToBlack.Fade(true);
            this.bossFade.Fade(true);
            yield return new WaitForSeconds(this.fadeToBlack.FadeTime / 2f);

            //Destroy the boss if it exists after the transition
            Boss b = FindObjectOfType<Boss>();
            if (b != null)
            {
                Destroy(b.gameObject);
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

        /// <summary>
        /// Stops incoming waves and starts the boss fight
        /// </summary>
        private IEnumerator<YieldInstruction> StartBossFight()
        {
            this.bossFight = true;
            this.asteroidController.StopWave();
            Destroy(this.asteroidController.gameObject);
            yield return new WaitForSeconds(this.bossDelay);
            
            this.bossFade.Fade();
            Instantiate(this.boss, this.bossSpawn, Quaternion.identity);
        }

        /// <summary>
        /// Full enemy wave destroyed event
        /// </summary>
        public void WaveDestroyed()
        {
            this.score *= 2;

            if (this.player.Level < Player.MAX_LEVEL)
            {
                Instantiate(this.powerup, this.powerupSpawn, Quaternion.identity);
                Debug.Log("A powerup has been created!");
            }
        }
        #endregion

        #region Functions
        //Get current Game Mode
        private void Awake() => this.mode = GameLogic.GameMode;

        private void Start()
        {
            this.background.StartMovement(AccelerationMovement.MovementMode.APPROACH);
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