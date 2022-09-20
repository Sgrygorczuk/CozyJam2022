using System;
using UnityEngine;

public class PlayerTalkResult : MonoBehaviour
{

    [SerializeField] private GameObject item;
    private string _itemName;
    private static bool _isHoldingItem;
    private SpriteRenderer _spriteRenderer;
    private int _result;
    private bool _isTransformed; 

    private void Start()
    {
        _spriteRenderer = transform.Find("PickedUpObject").transform.Find("Object").GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    public void PlayerAction(int result)
    {
        _result = result;
        switch (result)
        {
            case 4:
                DropItem();
                break;
            case 12:
            {
                Transform();
                break;
            }
        }
    }
    
    public int GetResult()
    {
        return _result;
    }
    

    //Result = 1
    public void Push()
    {
        
    }

    //Result = 4
    private void DropItem()
    {
        if (!_isHoldingItem) return;
        var instance = Instantiate(item, transform.position, Quaternion.identity);
        
        instance.GetComponent<PickUpObject>().SetNameAndSprite(_itemName, _spriteRenderer.sprite);
        _isHoldingItem = false;
        _spriteRenderer.enabled = false; 
    }
    
    //Result = 6 
    private void PlaySong()
    {
        
    }

    //Result 8
    public void PickItem(string itemName, Sprite itemSprite)
    {
        if (_isHoldingItem) return;
        _itemName = itemName;
        _spriteRenderer.sprite = itemSprite;
        _isHoldingItem = true;
        _spriteRenderer.enabled = true;
    }

    //Result 12
    private void Transform()
    {
        _isTransformed = !_isTransformed;
    }

    public bool GetIsTransformed()
    {
        return _isTransformed;
    }

    //Result 18
    public Sprite GiveItemSprite()
    {
        return _spriteRenderer.sprite;
    }

    public string GiveItemName()
    {
        return _itemName;
    }

    public void GiveObjectAway()
    {
        if (_isHoldingItem)
        {
            _isHoldingItem = false;
            _spriteRenderer.enabled = false;
        }
    }
}
