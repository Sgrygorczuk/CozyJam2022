using Player;
using UnityEngine;

/// <summary>
/// The Bush object can be hit once and will return a preFab of a berry for the player
/// to pick up
/// </summary>
public class Bush : GenericObject 
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    [Header("Bush Vars")]
    //Holds PreFab of the Berry 
    [SerializeField] private GameObject preFab;
    //Checks if the tree was shaken, if so no more berries 
    private bool _broken;

    //==================================================================================================================
    // Trigger Override  
    //==================================================================================================================
    
    protected override void TriggerMethod(Collider2D col)
    {
        //Checks if the bush hasn't been hit and it's the right song 
        if (_broken || col.transform.parent.GetComponent<PlayerTalkResult>().GetResult() != 1) return;
        //Breaks the bush
        _broken = true;
        //Changes sprite 
        SpriteRenderer.sprite = sprites[0];
        //Calculates where to place the berry 
        var vector = transform.position + 2 * Vector3.down;
        //Creates the berry 
        Instantiate(preFab, vector, Quaternion.identity);
    }
    
}