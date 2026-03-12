using UnityEngine;
using MagmaLabs;
using MagmaLabs.Controllers;
using System.Collections.Generic;

namespace MagmaLabs.Animation
{
    public class MonodirectionalAccessoryAnimator : MonoBehaviour
    {
        [Header("Movement Source")]
        [SerializeField] private TopDown2DPlayerController playerController;
        [SerializeField] private Rigidbody2D movementBody;
        [SerializeField] private Transform movementTransform;
        [SerializeField] private float movementEpsilon = 0.001f;

        [Header("Visual Target")]
        [SerializeField] private SpriteRenderer targetRenderer;
        [SerializeField] private GameObject accessory;

        [Header("Frame Source")]
        [SerializeField] private Item item;
        [SerializeField] private bool useLeftFacingSprites;
        [SerializeField] private Sprite[] leftSprites;
        [SerializeField] private Sprite[] rightSprites;
        [SerializeField] private float secondsPerFrame = 0.1f;
        
        private Sprite[] spritesInUse;
        private Vector3 lastPosition;
        private Vector2 lastDirection;
        int currentFrame = 0;
        float timeSinceLastFrame = 0f;
        
        private const string DefaultSlot = "default";

        private void Awake()
        {
            CacheReferences();
            RefreshFramesFromCurrentSource();
            if (movementTransform != null)
            {
                lastPosition = movementTransform.position;
            }
        }

        private void OnValidate()
        {
            CacheReferences();
            RefreshFramesFromCurrentSource();
        }

        private void OnEnable()
        {
            if (playerController != null)
            {
                playerController.OnPlayerMove.AddListener(OnPlayerMove);
            }
        }

        private void OnDisable()
        {
            if (playerController != null)
            {
                playerController.OnPlayerMove.RemoveListener(OnPlayerMove);
            }
        }

        private void Update()
        {
            if (playerController != null)
            {
                return;
            }

            Vector2 direction = ResolveMovementDirection();
            if (direction.sqrMagnitude > movementEpsilon * movementEpsilon)
            {
                Animate(direction);
                lastDirection = direction;
            }
            else if (lastDirection.sqrMagnitude > movementEpsilon * movementEpsilon)
            {
                ApplyFacing(lastDirection.x);
            }
        }

        private void CacheReferences()
        {
            if (targetRenderer == null && accessory != null)
            {
                targetRenderer = accessory.GetComponent<SpriteRenderer>();
            }

            if (targetRenderer == null)
            {
                targetRenderer = GetComponent<SpriteRenderer>();
            }

            if (movementTransform == null)
            {
                movementTransform = transform.root;
            }
        }

        private Vector2 ResolveMovementDirection()
        {
            if (movementBody != null)
            {
                return movementBody.linearVelocity;
            }

            if (movementTransform == null)
            {
                return Vector2.zero;
            }

            Vector3 current = movementTransform.position;
            Vector3 delta = current - lastPosition;
            lastPosition = current;
            return new Vector2(delta.x, delta.y);
        }

        private void OnPlayerMove(Vector2 direction)
        {
            Animate(direction);
            lastDirection = direction;
        }

        private void Animate(Vector2 direction)
        {
            if (targetRenderer == null || spritesInUse == null || spritesInUse.Length == 0)
            {
                return;
            }

            targetRenderer.sprite = spritesInUse[currentFrame];
            ApplyFacing(direction.x);

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

        private void ApplyFacing(float directionX)
        {
            if (targetRenderer == null || Mathf.Abs(directionX) <= movementEpsilon)
            {
                return;
            }

            if(directionX > 0)
            {
                targetRenderer.flipX = useLeftFacingSprites;
            }
            else
            {
                targetRenderer.flipX = !useLeftFacingSprites;
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

            item = null;
            RefreshFramesFromCurrentSource();
        }

        public void SetItem(Item newItem, bool leftFacing)
        {
            item = newItem;
            useLeftFacingSprites = leftFacing;
            RefreshFramesFromCurrentSource();
        }

        public void SetItem(Item newItem)
        {
            item = newItem;
            RefreshFramesFromCurrentSource();
        }

        public void RefreshFramesFromCurrentSource()
        {
            CacheReferences();

            if (item != null && item.sprites != null && item.sprites.Count > 0)
            {
                if (TryGetDirectionalSprites(item, useLeftFacingSprites, out Sprite[] parsedSprites))
                {
                    spritesInUse = parsedSprites;
                }
                else
                {
                    Sprite fallback = item.DefaultSprite;
                    spritesInUse = fallback != null ? new Sprite[] { fallback } : leftSprites;
                }
            }
            else
            {
                spritesInUse = useLeftFacingSprites ? leftSprites : rightSprites;
            }

            currentFrame = 0;
            timeSinceLastFrame = 0f;

            if (targetRenderer != null && spritesInUse != null && spritesInUse.Length > 0)
            {
                targetRenderer.sprite = spritesInUse[0];
            }
        }

        private static bool TryGetDirectionalSprites(Item sourceItem, bool leftFacing, out Sprite[] sprites)
        {
            List<Sprite> collected = new List<Sprite>();
            string[] prefixes = leftFacing
                ? new[] { "left_", "l_", "left", "l" }
                : new[] { "right_", "r_", "right", "r" };

            foreach (var kvp in sourceItem.sprites.Items)
            {
                if (kvp.value == null)
                {
                    continue;
                }

                string key = kvp.key == null ? string.Empty : kvp.key.ToLowerInvariant();
                if (HasAnyPrefix(key, prefixes))
                {
                    collected.Add(kvp.value);
                }
            }

            if (collected.Count == 0)
            {
                // if no directional keys were found, use every non-default sprite as animation frames
                foreach (var kvp in sourceItem.sprites.Items)
                {
                    if (kvp.value == null)
                    {
                        continue;
                    }

                    if (!string.Equals(kvp.key, DefaultSlot, System.StringComparison.OrdinalIgnoreCase))
                    {
                        collected.Add(kvp.value);
                    }
                }
            }

            if (collected.Count == 0 && sourceItem.DefaultSprite != null)
            {
                collected.Add(sourceItem.DefaultSprite);
            }

            sprites = collected.ToArray();
            return sprites.Length > 0;
        }

        private static bool HasAnyPrefix(string value, string[] prefixes)
        {
            for (int i = 0; i < prefixes.Length; i++)
            {
                if (value.StartsWith(prefixes[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }   


}
