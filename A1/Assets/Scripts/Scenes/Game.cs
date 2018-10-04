using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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
        internal Player player;
        [SerializeField]
        private AccelerationMovement background;
        [SerializeField]
        private float endGameWait;
        [SerializeField, Header("UI")]
        private GameObject pausePanel;
        [SerializeField]
        private FadeGraphics endFade, bossFade;
        [SerializeField]
        private Text scoreLabel, gameoverLabel;
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
        private int score;
        private bool bossFight;
        private WaveController asteroidController, enemyController;
        #endregion

        #region Properties
        /// <summary>
        /// If the Game has been ended (either winning or losing)
        /// </summary>
        public bool GameEnded { get; private set; }
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
            if (this.asteroidController != null) { Destroy(this.asteroidController); }
            this.GameEnded = true;
            this.player.Controllable = false;

            //Start the transition Coroutine
            StartCoroutine(won ? WinTransition() : LoseTransition());
        }

        /// <summary>
        /// Winning endgame coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator<YieldInstruction> WinTransition()
        {
            //Write password file
            File.WriteAllText(Path.Combine(Application.dataPath, @"..\Password.txt"), "Emilie sucks.", Encoding.ASCII);

            //Wait for the endgame
            yield return new WaitForSeconds(this.endGameWait);

            //Update UI
            this.gameoverLabel.text = "Congratulations!";

            //Fade out screen
            this.background.StartMovement(AccelerationMovement.MovementMode.ACCELERATE);
            this.player.GetComponent<AccelerationMovement>().StartMovement(AccelerationMovement.MovementMode.ACCELERATE);
            this.endFade.Fade();
            yield return new WaitForSeconds(this.endFade.FadeTime);

            //Make buttons interactable
            this.endFade.ToggleSelectables();
        }

        /// <summary>
        /// Losing endgame coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator<YieldInstruction> LoseTransition()
        {
            //Update UI and fade
            this.endFade.Fade(true);
            yield return new WaitForSeconds(this.endFade.FadeTime / 2f);

            //Destroy the boss if it exists after the transition
            Boss b = FindObjectOfType<Boss>();
            if (b != null)
            {
                Destroy(b.gameObject);
            }

            //Make buttons interactable
            this.endFade.ToggleSelectables();
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
            if (!GameLogic.IsHard)
            {
                this.asteroidController.StopWave();
                Destroy(this.asteroidController.gameObject);
            }
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

        /// <summary>
        /// Pause event
        /// </summary>
        /// <param name="state">Current game pause state</param>
        public void OnPause(bool state) => this.pausePanel.SetActive(state);

        /// <summary>
        /// Resume button press
        /// </summary>
        public void OnResume() => GameLogic.IsPaused = false;

        /// <summary>
        /// Return to menu button press
        /// </summary>
        public void OnReturnToMenu()
        {
            GameLogic.IsPaused = false;
            GameLogic.LoadScene(GameScenes.MENU);
        }

        /// <summary>
        /// Restarts a new game
        /// </summary>
        public void OnRestart() => GameLogic.LoadScene(GameScenes.GAME);
        #endregion

        #region Functions
        //Add OnPause listener
        private void Awake() => GameLogic.OnPause += OnPause;

        private void Start()
        {
            this.background.StartMovement(AccelerationMovement.MovementMode.APPROACH);
            this.asteroidController = Instantiate(this.asteroids).GetComponent<AsteroidWaveController>();
            this.asteroidController.StartWave();
            StartRandomController();
        }

        private void Update()
        {
            if (this.GameEnded)
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

        //Remove the OnPause event
        private void OnDestroy() => GameLogic.OnPause -= OnPause;
        #endregion
    }
}