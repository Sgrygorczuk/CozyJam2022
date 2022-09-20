using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    [SerializeField] private Sprite[] resultSprites;

    enum Type
    {
        Seed,
        Pumpkin,
        Berry,
        Acorn
    }

    [SerializeField] private Type type;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Collide");
        print(col.name);
        if (col.CompareTag($"PickUp"))
        {
            print(col.GetComponent<PickUpObject>().GetName());
            if (col.GetComponent<PickUpObject>().GetName() == "Seed" && type == Type.Seed)
            {
                PerformAction(col, 0);
            }
            else if (col.GetComponent<PickUpObject>().GetName() == "Acorn" && type == Type.Acorn)
            {
                PerformAction(col, 1);
            }
            else if (col.GetComponent<PickUpObject>().GetName() == "Berry" && type == Type.Berry)
            {
                PerformAction(col, 2);
            }
            else if (col.GetComponent<PickUpObject>().GetName() == "Pumpkin" && type == Type.Pumpkin)
            {
                PerformAction(col, 3);
            }
        }
    }

    private void PerformAction(Collider2D col, int place)
    {
        Destroy(col.gameObject);
        _spriteRenderer.sprite = resultSprites[place];
        _boxCollider2D.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        print(other.name);
    }
}
