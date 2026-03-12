using System.Collections.Generic;
using UnityEngine;
using MagmaLabs;
using MagmaLabs.Controllers;

namespace MagmaLabs.Animation
{
    public class MonodirectionalCharacterAnimator : MonoBehaviour
    {
    
        [SerializeField] private TopDown2DPlayerController playerController;
        [SerializeField] private SpriteRenderer sr;
        [Header("Drag sprites below")]
        [SerializeField] private bool useLeftFacingSprites;

        [ShowIf("useLeftFacingSprites", true)]
        [SerializeField] private Sprite[] leftSprites;
        [ShowIf("useLeftFacingSprites", false)]
        [SerializeField] private Sprite[] rightSprites;
        [SerializeField] private float secondsPerFrame = 0.1f;
        
        public Sprite[] spritesInUse;
        int currentFrame = 0;
        float timeSinceLastFrame = 0f;

        private void Start()
        {
            if (playerController != null)
            {
                playerController.OnPlayerMove.AddListener(OnPlayerMove);
            }
            spritesInUse = useLeftFacingSprites ? leftSprites : rightSprites;
            // Set initial sprite if available
            if (sr != null && spritesInUse.Length > 0)
            {
                sr.sprite = spritesInUse[0];
            }
        }

        public void SetItem(Item item)
        {
            // populate left/right sprite arrays from the item's dictionary
            if (item == null || item.sprites == null){
                Debug.LogWarning("SetItem called with null item or no sprites dictionary");
                return;
            }

            Debug.Log($"Setting item with {item.sprites.Count} sprites");

            var leftList = new List<Sprite>();
            var rightList = new List<Sprite>();
            var allSprites = new List<Sprite>();

            foreach (var kvp in item.sprites.Items)
            {
                Sprite s = kvp.value;
                if (s == null)
                {
                    Debug.LogWarning($"Null sprite in item.sprites for key: {kvp.key}");
                    continue;
                }

                allSprites.Add(s);
                string keyName = kvp.key.ToLower();
                Debug.Log($"Processing sprite: {s.name} with key: {kvp.key} (lowercase: {keyName})");
                if (keyName.Contains("left"))
                {
                    leftList.Add(s);
                    Debug.Log($"Added to left: {s.name} (key: {kvp.key})");
                }
                else if (keyName.Contains("right"))
                {
                    rightList.Add(s);
                    Debug.Log($"Added to right: {s.name} (key: {kvp.key})");
                }
                else
                {
                    Debug.Log($"Key '{kvp.key}' doesn't contain 'left' or 'right'");
                }
            }

            // If no directional sprites found, use all sprites for both directions
            if (leftList.Count == 0 && rightList.Count == 0 && allSprites.Count > 0)
            {
                Debug.Log("No directional sprites found, using all sprites for both left and right");
                leftSprites = allSprites.ToArray();
                rightSprites = allSprites.ToArray();
            }
            else
            {
                leftSprites = leftList.ToArray();
                rightSprites = rightList.ToArray();
            }

            Debug.Log($"Final arrays: leftSprites={leftSprites.Length}, rightSprites={rightSprites.Length}");

            // update current spritesInUse in case animation is already running
            spritesInUse = useLeftFacingSprites ? leftSprites : rightSprites;

            // Set initial sprite immediately
            if (sr != null && spritesInUse.Length > 0)
            {
                sr.sprite = spritesInUse[0];
                currentFrame = 0; // Reset animation frame
                Debug.Log($"Set initial sprite: {sr.sprite.name}");
            }
            else
            {
                Debug.LogWarning("No sprites to display - sr is null or spritesInUse is empty");
            }
        }

        public void OnPlayerMove(Vector2 direction){
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
    }   


}