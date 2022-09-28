using UnityEngine;

/// <summary>
/// Generic Object is used to give any interactable object the shake animation
/// access to Sprite Renderer and any number of sprites that they may change into
///
/// It also holds a Trigger Function that will be overwritten by different objects 
/// </summary>
public class GenericObject : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    //Holds the sprite renderer 
    protected SpriteRenderer SpriteRenderer;
    //Holds the animator 
    private Animator _animator;
    
    [Header("Generic Object Vars")]
    //Holds all the sprites 
    [SerializeField] protected Sprite[] sprites;
    //Tells us if the object can be transparent 
    [SerializeField] private bool shouldBeTransparent;

    //==================================================================================================================
    // Base Methods 
    //==================================================================================================================

    //Connects the sprite renderer and animator 
    private void Start()
    {
        SpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        OnStarting();
    }
    
    //Used to add any functionality to the start function without over writing the installations 
    protected virtual void OnStarting(){}
    
    //==================================================================================================================
    // Trigger Methods  
    //==================================================================================================================

    //Checks if the player sings at the object if they did shake the object and execute the Trigger Method 
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Checks if the player walked into the object, if it's a large inanimate object we 
        //will change color to be see through 
        if (col.CompareTag($"User"))
        {
            if (col.CompareTag($"User") && shouldBeTransparent) { UpdateColor(0.5f); }
        }

        //Checks if the player sang the song if so shake and react appropriately 
        if (!col.CompareTag("Player")) return;
        _animator.Play($"Shake");
        TriggerMethod(col);
    }

    //Trigger Method that each interactable object will override 
    protected virtual void TriggerMethod(Collider2D col) {}

    //Checks if player walked out of the inanimate object and returns to color back 
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag($"User") && shouldBeTransparent) { UpdateColor(1); }
    }

    //Changes the sprite renderer color to whatever is passed in 
    private void UpdateColor(float alpha)
    {
        SpriteRenderer.color = new Color(1, 1, 1, alpha);
    }
}
