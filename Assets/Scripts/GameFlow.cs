using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    //==== Control Scripts 
    private TalkController _talkController;
    private PlayerWalk _playerWalk;
    private PlayerTalk _playerTalk;
    private Animator _fadeAnimator;      //Allows us to fade in and out 
    private GameObject _gameCanvas;
    private GameObject _popUp;
    private TextMeshProUGUI _popUpText;

        private enum Quests
    {
        Tree, 
        Flowers,
        Acorns,
        Berries,
        Pumpkins,
        BearPumpkin,
    }

    [SerializeField] private List<NPC> npcs = new List<NPC>();
    private int[] _scores = new int[10];

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
    // Functions 
    //==================================================================================================================
    
    // Start is called before the first frame update
    private void Start()
    {
        //_fadeAnimator = GameObject.Find($"Fade Canvas").GetComponent<Animator>();
        _talkController = GameObject.Find($"Dialogue_Canvas").GetComponent<TalkController>();
        _playerTalk = GameObject.Find($"Player").GetComponent<PlayerTalk>();
        _playerWalk = GameObject.Find($"Player").GetComponent<PlayerWalk>();
        _gameCanvas = GameObject.Find("GameCanvas");
        _popUp = GameObject.Find("GameCanvas").transform.Find("PopUp").gameObject;
        _popUpText = _popUp.transform.Find("PopUpText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public void Update()
    {
        //Checks if the player is playing a desktop version if they are enables them to use ESC to quit out of the game 
        if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.LinuxPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        
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

    //Starts the game by fading from dark and starting the conversation 
    private IEnumerator StartGame()
    {
        _gameCanvas.SetActive(false);
            //_fadeAnimator.Play("ScreenFade");
        yield return new WaitForSeconds(1);
        _talkController.NextSentence();
    }
    
    //Exits talking and goes back to the player 
    public void EndDialogue()
    {
        _currentState = GameState.PlayerMoving;
        _gameCanvas.SetActive(true);
    }

    public void EndGame()
    {
        _currentState = GameState.End;
        StartCoroutine(Wipe());   
    }
    
    //Start the talking section 
    public void EnterDialogue(string[] sentences, Sprite sprite)
    {
        _talkController.LoadText(sentences, sprite);
        _gameCanvas.SetActive(false);
        _talkController.SetCanvas(true);
        _talkController.NextSentence();
        _currentState = GameState.TalkTime;
    }

    //Performs the animations and once it fade out we go to the next screen 
    private IEnumerator Wipe()
    {
        //_fadeAnimator.Play("ScreenFadeIn");
        yield return new WaitForSeconds(0.95f);
        SceneManager.LoadScene($"TBC");
    }

    public void AddTree()
    {
        _scores[(int) Quests.Tree]++;
        if (_scores[(int)Quests.Tree] == 13)
        {
            npcs[(int)Quests.Tree].QuestComplete();
        }
        var text = "Leaves shaken of trees " + _scores[(int)Quests.Tree] + "/" + "13";
        StartCoroutine(PopUp(text));
    }

    public void AddFlower()
    {
        _scores[(int) Quests.Flowers]++;
        if (_scores[(int)Quests.Flowers] == 6)
        {
            npcs[(int)Quests.Flowers].QuestComplete();
        }
        var text = "Flowers planted  " + _scores[(int)Quests.Flowers] + "/" + "6";
        StartCoroutine(PopUp(text));
    }
    
    public void AddPumpkin()
    {
        _scores[(int) Quests.Pumpkins]++;
        if (_scores[(int)Quests.Pumpkins] == 5)
        {
            npcs[(int)Quests.Pumpkins].QuestComplete();
        }
        var text = "Spooky Pumpkins Carved  " + _scores[(int)Quests.Pumpkins] + "/" + "5";
        StartCoroutine(PopUp(text));
    }
    
    public void AddAcorn()
    {
        _scores[(int) Quests.Acorns]++;
        if (_scores[(int)Quests.Acorns] == 3)
        {
            npcs[(int)Quests.Acorns].QuestComplete();
        }
        var text = "Nuts scurried away " + _scores[(int)Quests.Acorns] + "/" + "3";
        StartCoroutine(PopUp(text));
    }
    
    public void AddBerry()
    {
        _scores[(int) Quests.Berries]++;
        if (_scores[(int)Quests.Berries] == 3)
        {
            npcs[(int)Quests.Berries].QuestComplete();
        }
        var text = "Berries for winter collected  " + _scores[(int)Quests.Berries] + "/" + "3";
        StartCoroutine(PopUp(text));
    }
    
    public void AddBearPumpkin()
    {
        _scores[(int) Quests.BearPumpkin]++;
        if (_scores[(int)Quests.BearPumpkin] == 1)
        {
            npcs[(int)Quests.BearPumpkin].QuestComplete();
        }
        var text = "Pumpkin Pie Materials Delivered  " + _scores[(int)Quests.BearPumpkin] + "/" + "1";
        StartCoroutine(PopUp(text));
    }
    
    private IEnumerator PopUp(string text)
    {
        _popUpText.text = text;
        _popUp.SetActive(true);
        yield return new WaitForSeconds(3);
        _popUp.SetActive(false);
    }
    
    
}
