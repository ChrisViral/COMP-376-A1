using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Restricts the field of view of a Camera to the given aspect ratio
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraAspectRatio : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private float aspectRatio;
        #endregion

        #region Functions
        private void Awake()
        {
            //Set camera aspect ratio as needed
            Camera cam = this.gameObject.GetComponent<Camera>();
            float variance = this.aspectRatio / Camera.main.aspect;
            if (variance < 1.0f)
            {
                cam.rect = new Rect((1.0f - variance) / 2.0f, 0f, variance, 1.0f);
            }
            else
            {
                variance = 1.0f / variance;
                cam.rect = new Rect(0, (1.0f - variance) / 2.0f, 1.0f, variance);
            }
        }
        #endregion
    }
}