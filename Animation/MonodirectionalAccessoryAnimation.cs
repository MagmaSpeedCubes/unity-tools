using UnityEngine;
using MagmaLabs;
using MagmaLabs.Controllers;

namespace MagmaLabs.Animation
{
    public class MonodirectionalAccessoryAnimator : MonoBehaviour
    {
        
        [SerializeField] private TopDown2DPlayerController playerController;
        [SerializeField] private GameObject accessory;

        [Header("Drag sprites below")]
        [SerializeField] private bool useLeftFacingSprites;

        [ShowIf("useLeftFacingSprites", true)]
        [SerializeField] private Sprite[] leftSprites;
        [ShowIf("useLeftFacingSprites", false)]
        [SerializeField] private Sprite[] rightSprites;
        [SerializeField] private float secondsPerFrame = 0.1f;
        
        Sprite[] spritesInUse;
        SpriteRenderer sr;
        int currentFrame = 0;
        float timeSinceLastFrame = 0f;

        private void Start()
        {
            playerController.OnPlayerMove.AddListener(OnPlayerMove);
            spritesInUse = useLeftFacingSprites ? leftSprites : rightSprites;
            sr = accessory.GetComponent<SpriteRenderer>();


        }

        private void OnPlayerMove(Vector2 direction){
            //Debug.Log("Player moved: " + direction);

            
            if(direction.x > 0)
            {
                sr.sprite = spritesInUse[currentFrame];
                if(useLeftFacingSprites)
                {
                    sr.flipX = true;
                }
                else
                {
                    sr.flipX = false;
                }
            }
            else
            {
                sr.sprite = spritesInUse[currentFrame];
                if(useLeftFacingSprites)
                {
                    sr.flipX = false;
                }
                else
                {
                    sr.flipX = true;
                }
            }

            timeSinceLastFrame += Time.deltaTime;
            if(timeSinceLastFrame >= secondsPerFrame)
            {
                currentFrame++;
                if(currentFrame >= spritesInUse.Length)
                {
                    currentFrame = 0;
                }
                timeSinceLastFrame = 0f;
            }
        }
    
        public void SetSprites(Sprite[] sprites, bool leftFacing)
        {
            if (leftFacing)
            {
                leftSprites = sprites;
                useLeftFacingSprites = true;
            }
            else
            {
                rightSprites = sprites;
                useLeftFacingSprites = false;
            }
        }
    }   


}