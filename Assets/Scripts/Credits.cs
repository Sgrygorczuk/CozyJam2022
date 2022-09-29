using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    //Gets the player out of the credits into the Main Menu 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene("MainMenu"); }
    }
}
