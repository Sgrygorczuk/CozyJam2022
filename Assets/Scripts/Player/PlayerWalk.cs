using UnityEngine;

namespace Player
{
    /// <summary>
    /// Controls player walking and sprite 
    /// </summary>
    public class PlayerWalk : MonoBehaviour
    {
        //==================================================================================================================
        // Variables 
        //==================================================================================================================

        //============= Speed 
        private Rigidbody2D _rigidbody2D;                       //Controls the physics of the player  
        private float _x;                                       //Current X speed 
        private float _y;                                       //Current Y speed       
        [Header("Speed")]                           
        [SerializeField] private float speed = 2;               //What is the speed of regular movement 
        [SerializeField] private float transformedSpeed = 5;    //What is the speed when transformed 
    
        //============ Sprites 
        [Header("Player Sprites")]
        [SerializeField] private Sprite[] sprites;              //Different Sprites for directions 
        private SpriteRenderer _spriteRenderer;                 //Where sprites get displayed 
    
        //========= External Classes 
        private GameFlow _gameFlow;                             //Used to end Game 
        private PlayerTalkResult _playerTalkResult;             //Used to transform 
 
        //==================================================================================================================
        // Methods 
        //==================================================================================================================

        //Connects all of the components 
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
            _playerTalkResult = GetComponent<PlayerTalkResult>();
            _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        }

        //Used by Game Flow, get user inputs on Horizontal and Vertical and moves player accordingly 
        public void UpdateWalk()
        {
            //Get inputs from player 
            _x = Input.GetAxis("Horizontal");
            _y = Input.GetAxis("Vertical");
            //Checks what the speed should be and apply it to player
            var trueSpeed = _playerTalkResult.GetIsTransformed() ? transformedSpeed : speed; 
            _rigidbody2D.velocity = new Vector2(_x * trueSpeed, _y * trueSpeed);
            //Updates sprites 
            UpdateSprite();
        }
    
        //Changes the sprite 
        private void UpdateSprite()
        {
            //If player is transformed make them a leaf 
            if (_playerTalkResult.GetIsTransformed()) { _spriteRenderer.sprite = sprites[4]; }
            //Else switch between which direction the player is moving, if the player is not moving 
            //make them face the camera 
            else
            {
                _spriteRenderer.sprite = _x switch
                {
                    > 0 => sprites[0],
                    < 0 => sprites[1],
                    _ => _y > 0 ? sprites[2] : sprites[3]
                };
            }
        }

        //Checks if the player has walked into the end game spot 
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Finish")) return;
            //Make sure the player doesn't move after touching end point 
            _x = 0;
            _y = 0;
            _rigidbody2D.velocity = new Vector2(_x, _y);
            //End the game 
            _gameFlow.EndGame();
        }
    }
}
