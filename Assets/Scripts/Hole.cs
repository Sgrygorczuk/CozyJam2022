using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private GameFlow _gameFlow;

    [SerializeField] private Sprite[] resultSprites;

    enum Type
    {
        Seed,
        Pumpkin,
        Berry,
        Acorn,
        BearPumpkin,
        SeedTwo
    }

    [SerializeField] private Type type;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Collide");
        print(col.name);
        if (col.CompareTag($"PickUp"))
        {
            if (col.GetComponent<PickUpObject>().GetName() == "Seed" && type == Type.Seed)
            {
                PerformAction(col, 0);
                _gameFlow.AddFlower();
            }
            else if (col.GetComponent<PickUpObject>().GetName() == "Acorn" && type == Type.Acorn)
            {
                PerformAction(col, 1);
                _gameFlow.AddAcorn();
            }
            else if (col.GetComponent<PickUpObject>().GetName() == "Berry" && type == Type.Berry)
            {
                PerformAction(col, 2);
                _gameFlow.AddBerry();
            }
            else if (col.GetComponent<PickUpObject>().GetName() == "Pumpkin" && type == Type.Pumpkin)
            {
                PerformAction(col, 3);
                _gameFlow.AddPumpkin();
            }
            else if (col.GetComponent<PickUpObject>().GetName() == "Pumpkin" && type == Type.BearPumpkin)
            {
                PerformAction(col, 4);
                _gameFlow.AddBearPumpkin();
            }
            else if (col.GetComponent<PickUpObject>().GetName() == "Seed" && type == Type.SeedTwo)
            {
                PerformAction(col, 5);
                _gameFlow.AddBirdSeed();
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
