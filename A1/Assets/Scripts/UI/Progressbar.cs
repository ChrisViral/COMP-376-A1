using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.UI
{
    /// <summary>
    /// Progressbar
    /// </summary>
    [AddComponentMenu("UI/Progressbar")]
    public class Progressbar : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private RectTransform bar;
        [SerializeField]
        private Text label;
        [SerializeField, Range(0f, 1f)]
        private float progress = 1f;
        [SerializeField, HideInInspector]
        private bool scaling;

        //Private fields
        private Vector2 originalSize;
        #endregion

        #region Properties
        /// <summary>
        /// Progressbar's completion percentage, setting this will update the UI
        /// </summary>
        public float Progress
        {
            get { return this.progress; }
            set
            {
                this.progress = Mathf.Clamp01(value);
                this.bar.sizeDelta = new Vector2(this.progress * this.originalSize.x, this.originalSize.y);
                this.label.text = $"{(int)(this.progress * 100f)}%";
            }
        }
        #endregion

        #region Functions
        private void Start()
        {
            this.originalSize = this.bar.rect.size;
            this.Progress = this.progress;
        }

        private void Update()
        {
            //If scaling the bar
            if (this.scaling) { this.Progress = this.progress; }
        }
        #endregion
    }
}