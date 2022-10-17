using Player;
using UnityEngine;


/// <summary>
/// Used by any character the player can walk up to and talk to
/// The character can be a quest giver or just a random peron that talks 
/// </summary>
public class NPC : GenericObject
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================

    //========== Picked up Object 
    [Header("Picked Up Object")]
    [SerializeField] private bool isHoldingItem;                    //Tells us if the player is holding an object
    [SerializeField] private string itemName;                       //Holds the name of picked up object
    [SerializeField] private Sprite startPickedUpObjectSprite;      //Tells us what the object looks like 
    private SpriteRenderer _pickedUpObjectSpriteRenderer;           //How the object is displayed 
   
    //==========  Particle Effect 
    [Header("Particle Effect")]         
    private ParticleSystem _particleSystem;                         //Where the particle effect is displayed  
    [SerializeField] private Material[] particleEffectMaterials;    //The materials used for particle effects 

    //========== Talk Bubbles 
    [Header("Talk Bubble")]
    [SerializeField] private Sprite[] talkSprites;                  //Different types of bubbles 
    private SpriteRenderer _talkSpriteRenderer;                     //Where the bubble is displayed 
    
    //========= Game Controls 
    [Header("NPC Controls")]
    [SerializeField] private bool _questComplete;                                    //Tells us if the quest is complete 
    private GameFlow _gameFlow;                                     //Connect to Game Flow for update on quest 
    [SerializeField] private bool isQuestGiver;                     //Tells us if this NPC is a quest giver 
    
    //========== Display Items 
    [Header("Talking Vars")]
    [SerializeField] private string[] sentences;                //Holds all the lines the person will say 
    [SerializeField] private string[] questCompleteSentences;   //Holds all the lines the person will say 

    
    //==================================================================================================================
    // Overriden Methods  
    //==================================================================================================================
    protected override void OnStarting()
    {
        //Connects the held object 
        _pickedUpObjectSpriteRenderer = transform.Find("PickedUpObject").transform.Find("Object").GetComponent<SpriteRenderer>();
        //Turns if off if not holding anything 
        _pickedUpObjectSpriteRenderer.enabled = isHoldingItem;
        //Saves the sprite 
        _pickedUpObjectSpriteRenderer.sprite = startPickedUpObjectSprite;
        
        //Connect Game Flow and Particle Effect 
        _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        _particleSystem = transform.Find("ParticleSystem").GetComponent<ParticleSystem>();
       
        //Connect Bubble Sprite and set it based on if the NPC is a quest giver 
        _talkSpriteRenderer = transform.Find("TalkSprite").GetComponent<SpriteRenderer>();
        _talkSpriteRenderer.sprite = !isQuestGiver ? talkSprites[0] : talkSprites[1];
    }

    //Override the Action to perform something based on player song 
    protected override void TriggerMethod(Collider2D col)
    {
        switch (col.transform.parent.GetComponent<PlayerTalkResult>().GetResult())
        {
            //Takes object from Player 
            case 2:
            {
                GetObject(col);
                break;
            }
            //Gives object to Player
            case 3:
            { 
                GiveObject(col);
                break;
            }
            //Makes Happy Particle Effect Play
            case 9:
            {
                ReactNice();
                break;
            }
            //Makes Mean Particle Effect Play
            case 18:
            {
                ReactRude();
                break;
            }
            //Starts Conversation with NPC 
            case 27:
            {
                Talk();
                break;
            }
        }
    }

    //==================================================================================================================
    // Reaction Methods  
    //==================================================================================================================

    //Takes the object from Player to NPC
    private void GetObject(Component col)
    {
        if (isHoldingItem) return;
        var parent = col.transform.parent;
        //Make the player lose the object 
        parent.GetComponent<PlayerTalkResult>().Drop();
        //Copies the Name and Sprite from player 
        itemName = parent.GetComponent<PlayerTalkResult>().GiveItemName();
        _pickedUpObjectSpriteRenderer.sprite = parent.GetComponent<PlayerTalkResult>().GiveItemSprite();
        //Sets it so NPC is holding object and its visible 
        isHoldingItem = true;
        _pickedUpObjectSpriteRenderer.enabled = true;
    }

    //Gives the object that is being held to the player 
    private void GiveObject(Component col)
    {
        if (!isHoldingItem) return;
        //Gives the object's name and sprite to player 
        col.transform.parent.GetComponent<PlayerTalkResult>().PickItem(itemName, _pickedUpObjectSpriteRenderer.sprite);
        //Sets it so NPC is not holding and image is not visible 
        isHoldingItem = false;
        _pickedUpObjectSpriteRenderer.enabled = false;
    }

    //Initiates the talking sequence 
    private void Talk()
    {
        //If the NPC is a quest giver an quest is complete use second set of strings 
        if (isQuestGiver && _questComplete)
        {
            _gameFlow.EnterDialogue(questCompleteSentences);   
        }
        //Otherwise use the base set of strings 
        else
        {
            _gameFlow.EnterDialogue(sentences);
        }
    }

    //Sets the mean material to particle system and plays the effect 
    private void ReactRude()
    {
        _particleSystem.GetComponent<ParticleSystemRenderer>().material = particleEffectMaterials[0];
        _particleSystem.Play();
    }

    //Sets the nice material to particle system and plays the effect 
    private void ReactNice()
    {
        _particleSystem.GetComponent<ParticleSystemRenderer>().material = particleEffectMaterials[1];
        _particleSystem.Play();
    }

    //Sets the flag to complete the quest and changes the bubble sprite 
    public void QuestComplete()
    {
        _questComplete = true;
        _talkSpriteRenderer.sprite = talkSprites[2];
    }

}
