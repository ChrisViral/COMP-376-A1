using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Scrolls a background texture smoothly over time
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class BackgroundScroller : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private float speed;

        //Private fields
        private Material material;
        #endregion

        #region Functions
        //Get the material from the renderer
        private void Awake() => this.material = this.gameObject.GetComponent<Renderer>().material;

        //Scroll the texture by changing the texture offset
        private void Update() => this.material.SetTextureOffset("_MainTex", new Vector2(0f, (this.material.mainTextureOffset.y + (this.speed * Time.deltaTime)) % 1f));
        #endregion
    }
}