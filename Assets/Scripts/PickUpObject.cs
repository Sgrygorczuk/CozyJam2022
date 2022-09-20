using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] private string itemName = "Name";
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetNameAndSprite(string item, Sprite sprite)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        itemName = item;
        _spriteRenderer.sprite = sprite;
    }

    public string GetName()
    {
        return itemName;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.transform.parent.GetComponent<PlayerTalkResult>().GetResult() == 8)
        {
            col.transform.parent.GetComponent<PlayerTalkResult>().PickItem(itemName, _spriteRenderer.sprite);
            Destroy(gameObject);
        }
    }
}
