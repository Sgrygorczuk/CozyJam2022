using UnityEngine;

/// <summary>
/// Is the destination where you can place items, if the right items is placed
/// you get to progress the game 
/// </summary>
public class Hole : GenericObject
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //Box collider will be turned off when the right item is delivered 
    private BoxCollider2D _boxCollider2D;
    //Used to call Game Flow to update the progress of quest 
    private GameFlow _gameFlow;
    
    //Types of items that can be delivered 
    private enum Type
    {
        Seed,
        Pumpkin,
        Berry,
        Acorn,
        BearPumpkin,
        SeedTwo
    }
    
    [Header("Hole Vars")]
    //What type of item is it
    [SerializeField] private Type type;
    
    //==================================================================================================================
    // Variables 
    //==================================================================================================================

    //On start we connect to Game Flow to update progress and box collider that will turn off after 
    //completing quest 
    protected override void OnStarting()
    {
        _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    //Checks for any item that was placed on top of the hole,
    //If it's the correct item change the whole to complete state and update Game Flow
    //To content game progress 
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag($"PickUp")) return;
        if (col.GetComponent<PickUpObject>().GetName() == "Seed" && type == Type.Seed)
        {
            PerformAction(col, (int)Type.Seed);
            _gameFlow.AddQuest(GameFlow.Quests.Flowers);
        }
        else if (col.GetComponent<PickUpObject>().GetName() == "Acorn" && type == Type.Acorn)
        {
            PerformAction(col, (int)Type.Acorn);
            _gameFlow.AddQuest(GameFlow.Quests.Acorns);
        }
        else if (col.GetComponent<PickUpObject>().GetName() == "Berry" && type == Type.Berry)
        {
            PerformAction(col, (int)Type.Berry);
            _gameFlow.AddQuest(GameFlow.Quests.Berries);
        }
        else if (col.GetComponent<PickUpObject>().GetName() == "Pumpkin" && type == Type.Pumpkin)
        {
            PerformAction(col, (int)Type.Pumpkin);
            _gameFlow.AddQuest(GameFlow.Quests.Pumpkins);
        }
        else if (col.GetComponent<PickUpObject>().GetName() == "Pumpkin" && type == Type.BearPumpkin)
        {
            PerformAction(col, (int)Type.BearPumpkin);
            _gameFlow.AddQuest(GameFlow.Quests.BearPumpkin);
        }
        else if (col.GetComponent<PickUpObject>().GetName() == "Seed" && type == Type.SeedTwo)
        {
            PerformAction(col, (int)Type.SeedTwo);
            _gameFlow.AddQuest(GameFlow.Quests.BirdSeed);
        }
    }

    //Turns off the collider of the hol,e changes the sprite 
    //And destroys the placed down item 
    private void PerformAction(Component col, int place)
    {
        Destroy(col.gameObject);
        SpriteRenderer.sprite = sprites[place];
        _boxCollider2D.enabled = false;
    }
}