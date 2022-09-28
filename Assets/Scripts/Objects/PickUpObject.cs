using Player;
using UnityEngine;

/// <summary>
/// This is used for the object that are on the ground that the player can pick up
/// and creates new ones when it's dropped 
/// </summary>
public class PickUpObject : GenericObject
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    [Header("Picked Up Object")]
    //Holds the name so that we can check if it's the correct object for the goal 
    [SerializeField] private string itemName = "Name";

    //==================================================================================================================
    // Methods  
    //==================================================================================================================

    //When the game object is created passes the data from the player to the newly created game object 
    public void SetNameAndSprite(string item, Sprite sprite)
    {
        SpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        itemName = item;
        SpriteRenderer.sprite = sprite;
    }

    //Returns the name of the object so we can check if it's the one that the goal zone requires 
    public string GetName()
    {
        return itemName;
    }
    
    //Checks if the player performed the correct song, copies the data to the player and destroys the current 
    //game object 
    protected override void TriggerMethod(Collider2D col)
    {
        if (col.transform.parent.GetComponent<PlayerTalkResult>().GetResult() != 8) return;
        col.transform.parent.GetComponent<PlayerTalkResult>().PickItem(itemName, SpriteRenderer.sprite);
        Destroy(gameObject);
    }
}