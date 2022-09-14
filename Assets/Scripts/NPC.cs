using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    
    private string _itemName;
    private static bool _isHoldingItem;
    private SpriteRenderer _spriteRenderer;
    
    //========== Display Items 
    public string[] sentences;      //Holds all the lines the person will say 
    private GameFlow _gameFlow;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = transform.Find("PickedUpObject").transform.Find("Object").GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
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
                    ReactRude();
                    break;
                }
                case 9:
                {
                    ReactNice();
                    break;
                }
                case 12:
                {
                    //IDK
                    break;
                }
                case 18:
                {
                    GiveObject(col);
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
        if (!_isHoldingItem)
        {
            var parent = col.transform.parent;
            parent.GetComponent<PlayerTalkResult>().GiveObjectAway();
            _itemName = parent.GetComponent<PlayerTalkResult>().GiveItemName();
            _spriteRenderer.sprite = parent.GetComponent<PlayerTalkResult>().GiveItemSprite();
            _isHoldingItem = true;
            _spriteRenderer.enabled = true;   
        }
    }

    private void GiveObject(Component col)
    {
        if (_isHoldingItem)
        {
            col.transform.parent.GetComponent<PlayerTalkResult>().PickItem(_itemName, _spriteRenderer.sprite);
            _isHoldingItem = false;
            _spriteRenderer.enabled = false;   
        }   
    }

    private void Talk()
    {
        _gameFlow.EnterDialogue(sentences, _spriteRenderer.sprite);
    }

    private void ReactRude()
    {
        
    }

    private void ReactNice()
    {
        
    }
    
}
