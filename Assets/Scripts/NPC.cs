using UnityEngine;

public class NPC : MonoBehaviour
{
    
    [SerializeField] private string itemName;
    [SerializeField] private bool isHoldingItem;
    [SerializeField] private Sprite startSprite;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _particleSystem;
    [SerializeField] private Material[] material;
    [SerializeField] private bool isQuestGiver;
    [SerializeField] private Sprite[] talkSprites;
    private SpriteRenderer _talkSpriteRenderer;
    private bool _QuestComplete;

    //========== Display Items 
    [SerializeField] private string[] sentences;      //Holds all the lines the person will say 
    [SerializeField] private string[] questCompleteSentences;      //Holds all the lines the person will say 
    private GameFlow _gameFlow;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = transform.Find("PickedUpObject").transform.Find("Object").GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = isHoldingItem;
        _spriteRenderer.sprite = startSprite;
        _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        _particleSystem = transform.Find("ParticleSystem").GetComponent<ParticleSystem>();
        _talkSpriteRenderer = transform.Find("TalkSprite").GetComponent<SpriteRenderer>();

        _talkSpriteRenderer.sprite = !isQuestGiver ? talkSprites[0] : talkSprites[1];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            switch (col.transform.parent.GetComponent<PlayerTalkResult>().GetResult())
            {
                case 2:
                {
                    GetObject(col);
                    break;
                }
                case 3:
                {
                    GiveObject(col);
                    break;
                }
                case 9:
                {
                    ReactNice();
                    break;
                }
                case 18:
                {
                    ReactRude();
                    break;
                }
                case 27:
                {
                    Talk();
                    break;
                }
            }
        }
    }

    private void GetObject(Component col)
    {
        if (!isHoldingItem)
        {
            var parent = col.transform.parent;
            parent.GetComponent<PlayerTalkResult>().GiveObjectAway();
            itemName = parent.GetComponent<PlayerTalkResult>().GiveItemName();
            _spriteRenderer.sprite = parent.GetComponent<PlayerTalkResult>().GiveItemSprite();
            isHoldingItem = true;
            _spriteRenderer.enabled = true;   
        }
    }

    private void GiveObject(Component col)
    {
        if (isHoldingItem)
        {
            col.transform.parent.GetComponent<PlayerTalkResult>().PickItem(itemName, _spriteRenderer.sprite);
            isHoldingItem = false;
            _spriteRenderer.enabled = false;   
        }   
    }

    private void Talk()
    {
        if (isQuestGiver && _QuestComplete)
        {
            _gameFlow.EnterDialogue(questCompleteSentences, _spriteRenderer.sprite);   
        }
        else
        {
            _gameFlow.EnterDialogue(sentences, _spriteRenderer.sprite);
        }
    }

    private void ReactRude()
    {
        _particleSystem.GetComponent<ParticleSystemRenderer>().material = material[0];
        _particleSystem.Play();
    }

    private void ReactNice()
    {
        _particleSystem.GetComponent<ParticleSystemRenderer>().material = material[1];
        _particleSystem.Play();
    }

    public void QuestComplete()
    {
        _QuestComplete = true;
        _talkSpriteRenderer.sprite = talkSprites[2];
    }

}
