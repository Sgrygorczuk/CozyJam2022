using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls how the game plays 
/// </summary>
public class GameFlow : MonoBehaviour
{
    //==== Control Scripts 
    private TalkController _talkController; //Controls the Talk UI 
    private PlayerWalk _playerWalk;         //Controls player walking   
    private PlayerTalk _playerTalk;         //Control player talking 
    private GameObject _gameCanvas;         //Controls canvas that show player Queue 
    private GameObject _popUp;              //Shows player pop ups inside game canvas 
    private TextMeshProUGUI _popUpText;     //Controls the text in pop up 
    private GameObject _winSpot;            //Controls the place 
    [SerializeField] private string[] sentences;      //Holds all the lines the person will say 
    private Animator _cameraAnimator;       //Controls the start and end animations 
    

    //================== Quest Data  
    //Holds the different types of quests 
    public enum Quests
    {
        Tree, 
        Flowers,
        Acorns,
        Berries,
        Pumpkins,
        BearPumpkin,
        BirdSeed,
        BirdSeed2,
    }
    
    [SerializeField] private List<NPC> nonPlayableCharacters = new List<NPC>(); //Holds all the characters affected by quest 
    private readonly string[] _popUps = {                                       //Holds all the quest pop up texts 
        "Leaves shaken of trees ", "Flowers planted  ",
        "Nuts scurried away ", "Berries for winter collected  ", 
        "Spooky Pumpkins Carved  ", "Pumpkin Discreetly Delivered ",
        "Special Flower Deliver "
    };
    private readonly int[] _scores = new int[7];                                //Holds current counters in game 
    private readonly int[] _results = {13, 6, 3, 3, 5, 1, 1};                   //Holds the results each quest strives for 
    [SerializeField] private int winCounter;                                    //Keeps track of how many quests have been completed 
    
    //===== Game Flow 
    private enum GameState             
    {
        Start,            //Loads in the scene and set up the first talking section 
        PlayerMoving,     //The player is in control of moving 
        TalkTime,         //Talking sections 
        End,              //The end cut scene plays, and we exit to credits scene 
    }
    
    private GameState _currentState = GameState.Start;      //Keeps track of what state we're currently in
    
    //==================================================================================================================
    // Base Functions 
    //==================================================================================================================
    
    // Start is called before the first frame update
    private void Start()
    {
        //Connects all components adn disables the win spot 
        _talkController = GameObject.Find($"Dialogue_Canvas").GetComponent<TalkController>();
        _playerTalk = GameObject.Find($"Player").GetComponent<PlayerTalk>();
        _playerWalk = GameObject.Find($"Player").GetComponent<PlayerWalk>();
        _gameCanvas = GameObject.Find("GameCanvas");
        _popUp = GameObject.Find("GameCanvas").transform.Find("PopUp").gameObject;
        _popUpText = _popUp.transform.Find("PopUpText").GetComponent<TextMeshProUGUI>();
        _cameraAnimator = GameObject.Find("Player").transform.Find("Main Camera").GetComponent<Animator>();
        _winSpot = GameObject.Find("WinSpot");
        _winSpot.SetActive(false);
    }

    //Controls the game's actions based on the state it's in 
    public void Update()
    {
        //Checks what state the game is currently in and updates it 
        switch (_currentState)
        {
            case GameState.Start:
            {
                _currentState = GameState.TalkTime;
                StartCoroutine(StartGame());
                break;
            }
            case GameState.PlayerMoving:
            {
                _playerTalk.UpdateTalk();
                _playerWalk.UpdateWalk();
                break;
            }
            case GameState.TalkTime:
            {
                _talkController.TalkUpdate();
                break;
            }
            case GameState.End:
            {
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    //==================================================================================================================
    // Transition Functions 
    //==================================================================================================================


    //Starts the game by fading from dark and starting the conversation 
    private IEnumerator StartGame()
    {
        _gameCanvas.SetActive(false);
        yield return new WaitForSeconds(1);
        _talkController.NextSentence();
    }
    
    //Exits talking and goes back to the player 
    public void EndDialogue()
    {
        _currentState = GameState.PlayerMoving;
        _gameCanvas.SetActive(true);
    }

    //Start the end game sequence where the player gets to see the level then ends it after timer 
    public void EndGame()
    {
        _currentState = GameState.End;
        _gameCanvas.SetActive(false);
        _cameraAnimator.Play($"End");
        StartCoroutine(End());
    }

    //Ends the game and goes to the next scene 
    private static IEnumerator End()
    {
        yield return new WaitForSeconds(15.1f);
        SceneManager.LoadScene($"Credits");
    }
    
    //Starts the conversion w
    public void EnterDialogue(string[] lines)
    {
        //Updates UI
        _gameCanvas.SetActive(false);
        _talkController.SetCanvas(true);
        
        //Loads the data and starts the conversation 
        _talkController.LoadText(lines);
        _talkController.NextSentence();
        _currentState = GameState.TalkTime;
    }

    
    //==================================================================================================================
    // Quest Functions 
    //==================================================================================================================

    //Takes in the quest type and using the Data it increments whatever was done, prints out a statement 
    //for the player to update them on the progress and check if the quest was completed 
    public void AddQuest(Quests quests)
    {
        _scores[(int) quests]++;
        var text = _popUps[(int)quests] + _scores[(int)quests] + "/" + _results[(int)quests];
        StartCoroutine(PopUp(text));
        if (_scores[(int)quests] != _results[(int)quests]) return;
        nonPlayableCharacters[(int)quests].QuestComplete();
        WinCounter();

        //Accounts for the 2 Bird Quest Givers 
        if (quests == Quests.BirdSeed) { nonPlayableCharacters[(int) Quests.BirdSeed2].QuestComplete(); }
    }
    
    //Update count of completed quests if all 7 are finished the end goal opens up
    private void WinCounter()
    {
        winCounter++;
        if (winCounter != 7) return;
        _winSpot.SetActive(true);   
        EnterDialogue(sentences);
    }
    
    //Determines what the pop up will say and how long the pop up will remain on screen 
    private IEnumerator PopUp(string text)
    {
        _popUpText.text = text;
        _popUp.SetActive(true);
        yield return new WaitForSeconds(5);
        _popUp.SetActive(false);
    }

}
