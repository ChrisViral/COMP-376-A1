using System.Collections.Generic;
using SpaceShooter.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Scenes
{
    /// <summary>
    /// Menu flow controller
    /// </summary>
    [DisallowMultipleComponent]
    public class Menu : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private FadeGraphics fadeButtons, fadeDifficulty;
        [SerializeField]
        private Text helpText;
        #endregion

        #region Methods
        /// <summary>
        /// Start button event
        /// </summary>
        public void OnStart() => StartCoroutine(ToDifficulty());

        /// <summary>
        /// Fade to difficulty selection coroutine
        /// </summary>
        private IEnumerator<YieldInstruction> ToDifficulty()
        {
            //Toggle objects
            this.fadeButtons.ToggleSelectables();
            this.fadeDifficulty.ToggleGameObjects();
            yield return new WaitForEndOfFrame();
            
            //Fade groups
            this.fadeButtons.Fade();
            this.fadeDifficulty.Fade();
            yield return new WaitForSeconds(Mathf.Max(this.fadeButtons.FadeTime, this.fadeDifficulty.FadeTime));

            //Toggle new objects
            this.fadeButtons.ToggleGameObjects();
            this.fadeDifficulty.ToggleSelectables();
        }

        /// <summary>
        /// Fade to main menu coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator<YieldInstruction> FromDifficulty()
        {
            //Toggle objects
            this.fadeDifficulty.ToggleSelectables();
            this.fadeButtons.ToggleGameObjects();
            yield return new WaitForEndOfFrame();
            
            //Fade groups-
            this.fadeDifficulty.Fade();
            this.fadeButtons.Fade();
            yield return new WaitForSeconds(Mathf.Max(this.fadeButtons.FadeTime, this.fadeDifficulty.FadeTime));
            
            //Toggle new objects
            this.fadeDifficulty.ToggleGameObjects();
            this.fadeButtons.ToggleSelectables();
        }

        /// <summary>
        /// Help button event
        /// </summary>
        public void OnHelp()
        {
            //TODO: Implement help screen properly
            //this.fadeButtons.Fade();
            //this.fadePanel.Fade();
            this.helpText.text = "Git Gud.";
        }

        /// <summary>
        /// Exit button event
        /// </summary>
        public void OnExit() => GameLogic.Quit();

        /// <summary>
        /// Normal mode button event
        /// </summary>
        public void OnNormalMode()
        {
            GameLogic.Mode = GameMode.NORMAL;
            GameLogic.LoadScene(GameScenes.GAME);
        }

        /// <summary>
        /// Hard mode button event
        /// </summary>
        public void OnHardMode()
        {
            GameLogic.Mode = GameMode.HARD;
            GameLogic.LoadScene(GameScenes.GAME);
        }

        /// <summary>
        /// Return to main menu button event
        /// </summary>
        public void OnReturn() => StartCoroutine(FromDifficulty());
        #endregion
    }
}