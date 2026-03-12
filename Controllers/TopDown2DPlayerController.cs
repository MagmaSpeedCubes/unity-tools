using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace MagmaLabs.Controllers{
    public class TopDown2DPlayerController : PlayerController
    {

        private Vector2 direction;
        [Header("Drag the player rigidbody here")]
        public GameObject character;
        [SerializeField]private Character profile;
        public InputAction playerControls;
        [Header("Set the player speed here")]
        [SerializeField] private float speed = 8f;
        public UnityEvent<Vector2> OnPlayerMove;


        private void OnEnable()
        {
            playerControls.Enable();
            if(profile == null)
            {
                profile = ScriptableObject.CreateInstance<Character>();
            }
            LevelController.instance.levelRuntime.playerCharacter = profile;
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }


        private void FixedUpdate()
        {
            Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * speed;
            if(direction != Vector2.zero)
            {
                OnPlayerMove.Invoke(direction);
            }
            //Debug.Log("Direction: " + direction);
        }

        private void Update()
        {
            direction = playerControls.ReadValue<Vector2>();
            
        }
    }
}