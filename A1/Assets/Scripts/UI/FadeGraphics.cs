using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.UI
{
    /// <summary>
    /// Fades a collection of UI elements in and out of view
    /// </summary>
    [AddComponentMenu("UI/Fade Graphics")]
    public class FadeGraphics : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        internal Graphic[] graphics;
        [SerializeField]
        private Selectable[] selectables;
        [SerializeField, Header("Fading")]
        private float fadeTime;
        [SerializeField, Tooltip("Alpha value to fade to")]
        private float fadeTo;
        [SerializeField]
        private bool faded;

        //Private fields
        private float[] original;
        #endregion

        #region Properties
        /// <summary>
        /// Time taken to fade completely out/in
        /// </summary>
        public float FadeTime => this.fadeTime;

        /// <summary>
        /// If the graphic is currently faded
        /// </summary>
        public bool Faded => this.faded;
        #endregion

        #region Methods
        /// <summary>
        /// Toggles the fade of the group
        /// </summary>
        public void Fade(bool fast = false)
        {
            float time = this.fadeTime;
            if (fast) { time /= 2f; }

            if (this.faded)
            {
                //Restore all graphics to their original alpha values
                for (int i = 0; i < this.graphics.Length; i++)
                {
                    this.graphics[i].CrossFadeAlpha(this.original[i], time, true);
                }

                this.faded = false;
            }
            else
            {
                //Fade all graphics out of view
                foreach (Graphic g in this.graphics)
                {
                    g.CrossFadeAlpha(this.fadeTo, time, true);
                }

                this.faded = true;
            }
        }

        /// <summary>
        /// Toggles all the Selectable objects in this fade group to the opposite state
        /// </summary>
        public void ToggleSelectables()
        {
            //Toggle all selectables
            foreach (Selectable s in this.selectables)
            {
                s.interactable = !s.interactable;
            }
        }

        /// <summary>
        /// Toggles the selectable GameObjects on and off
        /// </summary>
        public void ToggleGameObjects()
        {
            foreach (Selectable s in this.selectables)
            {
                //Toggle active state of game objects
                GameObject go = s.gameObject;
                go.SetActive(!go.activeSelf);

                //If the objects should be faded, fade them back
                if (go.activeSelf && this.faded)
                {
                    foreach (Graphic g in this.graphics)
                    {
                        g.CrossFadeAlpha(this.fadeTo, 0f, true);
                    }
                }
            }
        }
        #endregion

        #region Functions
        private void Start()
        {
            //Get original alpha values for restoration
            this.original = new float[this.graphics.Length];
            for (int i = 0; i < this.original.Length; i++)
            {
                this.original[i] = this.graphics[i].color.a;
            }

            //Fade all graphics out of view if originally faded
            if (this.faded)
            {
                foreach (Graphic g in this.graphics)
                {
                    g.CrossFadeAlpha(this.fadeTo, 0f, true);
                }
            }
        }
        #endregion
    }
}