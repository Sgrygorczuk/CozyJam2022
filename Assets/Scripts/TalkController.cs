using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    /// <summary>
    /// This Script controls talking done in the dialogue portions  
    /// </summary>
    
    //========== Connections to Visuals 
    private AudioSource _audioSource;       //Plays the sound every time player hits space 
    private Canvas _dialogueCanvas;         //The canvas that holds all the images 
    private TextMeshProUGUI _dialogueText;  //The text box that displays the text 
    private Image _character;               //The sprite of the image of the character talking 
    private GameFlow _gameFlow;             //
    
    //=========== Internal Vars 
    public string[] sentences;          //Holds all the sentences this talk space offers 
    public int index;                   //Tells us which sentence we're at 
    private bool _breakOut;             //Tells us if the conversation is over or not 
    private bool _isTalking;            //Tells us if we're still in conversation, in case you want to skip the dialogue 
    public float dialogueSpeed;         //Tells us how fast the speed of the letters appearing should be 

    //==================================================================================================================
    // Functions 
    //==================================================================================================================
    
    //Connects all the components 
    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _dialogueCanvas = GetComponent<Canvas>();
        _dialogueText = GameObject.Find($"Dialogue_Canvas").transform.Find($"TalkText").GetComponent<TextMeshProUGUI>();
        //_character = GameObject.Find($"Dialogue_Canvas").transform.Find($"Panel").transform.Find($"Character").GetComponent<Image>();
        _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
    }
    
    //Allows player continue forward in the conversation 
    public void TalkUpdate()
    {
        if (!Input.GetButtonDown($"Sound1") && !Input.GetButtonDown($"Sound2") && !Input.GetButtonDown($"Sound3")) return;
        PlayAudio();
        //If we reached the end exit out of the talking bit 
        if (Done())
        {
            SetCanvas(false);
            _gameFlow.EndDialogue();
        }

        NextSentence();
    }

    //Plays the next sentence 
    private void PlayAudio()
    {
        _audioSource.Play();
    }
    
    //Loads in the new data from a Talk Zone and resets the index 
    public void LoadText(string[] newSentences, Sprite sprite)
    {
        index = 0;
        sentences = newSentences;
        //_character.sprite = sprite;
    }
    
    //Loads in the new data from a Talk Zone and resets the index 
    public void LoadText(string[] newSentences)
    {
        index = 0;
        sentences = newSentences;
        //_character.sprite = sprite;
    }

    //Tells us if we reached the end 
    private bool Done()
    {
        return index == sentences.Length;
    }

    //Sets the canvas to be on or off
    public void SetCanvas(bool state)
    {
        _dialogueCanvas.enabled = state;
    }
    

    //Checks if we can go to the next sentence if we can writes or if we're in the middle of the sentence we skip ahead
    public void NextSentence()
    {
        if (index <= sentences.Length - 1 && !_isTalking)
        {
            _dialogueText.text = "";
            _isTalking = true;
            StartCoroutine(WriteSentence());
        }
        else
        {
            _breakOut = true;
        }
    }

    //Parses through the sentences adding in one letter at a time and listens for if the player wants to move to next 
    //sentence 
    private IEnumerator WriteSentence()
    {
        //Goes through all the letters in the sentence 
        foreach (var dialogueTextText in sentences[index].ToCharArray())
        {
            //If the player actioned to move forward we stop writing this sentence and we move onto the next one 
            if (_breakOut)
            {
                //Fills out the rest of the sentence and preps is for the next one 
                _breakOut = false;
                _dialogueText.text = sentences[index];
                index++;
                _isTalking = false;
                yield break;
            }
            //Adds the letter 
            _dialogueText.text += dialogueTextText;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        //Once we finished all the letter stop adding letters and move forward 
        _isTalking = false;
        index++;
    }
}
