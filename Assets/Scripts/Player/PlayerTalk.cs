using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    /// <summary>
    /// This is the way player creates the notes, how they get displayed on screen and
    /// how the results get passed on  
    /// </summary>
    public class PlayerTalk : MonoBehaviour
    {
        //==================================================================================================================
        //Variables 
        //==================================================================================================================
        //======= Queue
        [SerializeField] private Sprite[] queueSprites;                 //Sprites shown in the Queue
        [SerializeField] private Image[] _queueImages = new Image[3];           //The Images that show the queue
        private Image _queueBar;                                        //Bar used to show the player time left 
        private Animator _queueAnimator;                                //Animator that works on Queue Images 
        private readonly List<int> _talkQueue = new List<int>();        //What is in the current Queue 
    
        //======= Notes 
        [SerializeField] private GameObject[] preFab;               //PreFab of the Notes 
        private Transform _noteSpawn;                               //Where they will be spawned 

        //======= Flags 
        private bool _hasPlayed;                //Tells us player played a note and needs to wait to play another 
        private bool _isCheatSheet;             //Tells us the cheat sheet is on 
        private bool _isCountingDown;           //Tells us the queue is active and counting down 
    
        //======= Misc 
        private GameObject _cheatSheetCanvas;       //canvas that turns on the cheat sheet 
        private PlayerTalkResult _playerTalkResult; //Where the song results are executed 
        private GameObject _triggerCollider;        //The collider that's sent at the end of the song 

        //======== Count Downs
        private float _currentTime = 0.75f;         //Current time on the count down
        private const float WaitTime = 0.75f;       //What we rest to 

        //==================================================================================================================
        // Start Methods 
        //==================================================================================================================

        // Start is called before the first frame update
        private void Awake()
        {
            //Where the notes will be instantiated   
            _noteSpawn = transform.Find("ShapeSpawn").transform;

            //Connects all of the Queue components and sets the bar to 0
            _queueBar = GameObject.Find("GameCanvas").transform.Find("QueueBar").GetComponent<Image>();
            _queueAnimator = GameObject.Find("GameCanvas").transform.Find("Queue").GetComponent<Animator>();
            _queueBar.fillAmount = 0;
        
            //Connects the cheat sheet canvas and sets it to invisible 
            _cheatSheetCanvas = GameObject.Find("CheatSheetCanvas").gameObject;
            _cheatSheetCanvas.SetActive(false);
        
            //Connect the pulse collider and sets it to off 
            _triggerCollider = transform.Find("Collider").gameObject;
            _triggerCollider.SetActive(false);
        
            //Connect Result 
            _playerTalkResult = GetComponent<PlayerTalkResult>();
        }

        //==================================================================================================================
        // Player Action Methods 
        //==================================================================================================================

        //Controls player action used through Game Flow 
        public void UpdateTalk()
        {
            //If player is looking at the cheat sheet they can only click ESC to leave it 
            if (_isCheatSheet) { CheatSheetAction(); }
            //If the player is transformed they can turn on the cheat sheet or return to normal form 
            else if (_playerTalkResult.GetIsTransformed()) { TransformAction(); }
            //Standard Inputs from Player 
            else
            {   
                StandardAction();
                UpdatingQueueCountDown();   
            }
        }

        //Turns the Cheat Sheet Off 
        private void CheatSheetAction()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            _isCheatSheet = false;
            _cheatSheetCanvas.SetActive(_isCheatSheet);
        }

        //Allows Player to Turn Cheat Sheet on or go to Standard Mode  
        private void TransformAction()
        {
            //Return to Normal Form 
            if ((Input.GetButtonDown($"Sound1") || (Input.GetButtonDown($"Sound2")) || (Input.GetButtonDown($"Sound3"))))
            {
                _playerTalkResult.PlayerAction(12);
            }

            //Checks if player turned on cheat sheet 
            TurnOnCheatSheet();
        }

        //Performs actions when in stand mode, can turn on cheat sheet, and sing 
        private void StandardAction()
        {
            //If the Queue is full perform the song and reset the queue 
            if (_talkQueue.Count == 3)
            {
                FullMove();
                return;
            }
        
            //Checks if player turned on cheat sheet 
            TurnOnCheatSheet();

            //Checks for player input and sings corresponding song 
            if (Input.GetButtonDown($"Sound1") && !_hasPlayed) { Action(0); }
            if (Input.GetButtonDown($"Sound2") && !_hasPlayed) { Action(1); }
            if (Input.GetButtonDown($"Sound3") && !_hasPlayed) { Action(2); }
        }

        //==================================================================================================================
        // Sub Actions 
        //==================================================================================================================

        //Checks if player clicked ESC to turn on cheat sheet, if they did turn it on 
        private void TurnOnCheatSheet()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            _isCheatSheet = true;
            _cheatSheetCanvas.SetActive(_isCheatSheet);
        }
    
        //Performs the action when any note is player 
        private void Action(int i)
        {
            //Updates the queue with the new note
            UpdateQueue(i);
            //Creates the note that will radiate from the player 
            var instantiate = Instantiate(preFab[i], transform.position, Quaternion.identity);
            instantiate.transform.parent = _noteSpawn;
            //Stops the player from spamming notes and displays counter till this song fails 
            _hasPlayed = true;
            _isCountingDown = true;
            _currentTime = WaitTime;
            _queueBar.fillAmount = 1;
            //Starts count down till when player can click again 
            StartCoroutine(_talkQueue.Count == 3 ? PlayTimer(1f) : PlayTimer(0.3f));
        }

        //==================================================================================================================
        // Queue
        //==================================================================================================================

        //Put updates the image and makes it visible, plays to song and adds it to the queue 
        private void UpdateQueue(int i)
        {
            _talkQueue.Add(i);
            _queueImages[_talkQueue.Count - 1].enabled = true;
            _queueImages[_talkQueue.Count - 1].sprite = queueSprites[i];
            _queueAnimator.Play($"Queue_" + _talkQueue.Count);
        }

        //Updates the bar till the player fails the song
        private void UpdatingQueueCountDown()
        {
            if (!_isCountingDown) return;
            //Lower the amount of time left 
            _currentTime -= Time.deltaTime;
            //Update the visual bar 
            _queueBar.fillAmount -= 1.0f / WaitTime * Time.deltaTime;
            if (!(_currentTime <= 0)) return;
            //Start to reset the queue 
            _hasPlayed = true;
            StartCoroutine(PlayTimer(1f));
            StartCoroutine(ResetQueue(true));
        }
    
        //Counts down till the player can play another note 
        private IEnumerator PlayTimer(float time)
        {
            yield return new WaitForSeconds(time);
            _hasPlayed = false;
        }

        //==================================================================================================================
        // Action Results 
        //==================================================================================================================

        //If the player played three notes, get the result of the notes and perform that action inside of 
        //Player Talk Result, set the collider to send out this wave of data to all nearby objects,
        //Resets the queue to start the next song 
        private void FullMove()
        {
            if (_talkQueue.Count != 3) return;
            _playerTalkResult.PlayerAction((_talkQueue[0] + 1) * (_talkQueue[1] + 1) * (_talkQueue[2] + 1));
            StartCoroutine(Collider());
            StartCoroutine(ResetQueue(false));
        }

        //Collider gets turned on for 0.1 sec and turned off so that near by object can interact 
        //with player song 
        private IEnumerator Collider()
        {
            _triggerCollider.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            _triggerCollider.SetActive(false);
        }
    
        //Resets the queue and updates the visuals 
        private IEnumerator ResetQueue(bool fail)
        {
            //Empties the queue 
            _talkQueue.Clear();
            //No more counting down 
            _isCountingDown = false;
            //Makes the count down bar size 0 
            _queueBar.fillAmount = 0;
            //Waits for few second 
            yield return new WaitForSeconds(0.6f);
            //Performs the Fail or Success Animation 
            _queueAnimator.Play(fail ? $"Fail" : $"Full");
            yield return new WaitForSeconds(0.3f);
            //Hides all of the images 
            foreach (var image in _queueImages) { image.enabled = false; }
        }

    }
}
