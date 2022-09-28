using System.Collections;
using UnityEngine;

/// <summary>
/// Used by the notes that the player instances to die after 1.1 sec 
/// </summary>
public class NoteDeath : MonoBehaviour
{
    //Initiates the Count Down 
    private void Start() { StartCoroutine(Death()); }

    //Waits till countdown is done then destroys the object 
    private IEnumerator Death() 
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
    }
}
