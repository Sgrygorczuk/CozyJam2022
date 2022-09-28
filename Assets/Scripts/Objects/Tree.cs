using System.Collections;
using Player;
using UnityEngine;

namespace Objects
{
    /// <summary>
    /// The Tree can be hit twice and each time it hits it changes the sprite 
    /// </summary>
    public class Tree : GenericObject
    {
        //==================================================================================================================
        // Variables 
        //==================================================================================================================
    
        //Keeps track of which sprite we're currently on
        private int _currentSprite ;
        //Checks if the tree just got pushed, if so pause for a second
        private bool _pushed;
        //Used to Update Game Flow about how many trees have been knocked over 
        private GameFlow _gameFlow;

        //==================================================================================================================
        // Base Methods 
        //==================================================================================================================

        //Randomizes the rotation of the tree and connects the tree to Game Flow controller 
        protected override void OnStarting()
        {
            transform.localScale = Random.Range(0, 2) == 1 ? new Vector3(-1,1,1) : new Vector3(1,1,1);
            _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        }

        //==================================================================================================================
        // Trigger Override  
        //==================================================================================================================

        //Actives the functions if the result is 1 
        protected override void TriggerMethod(Collider2D col)
        {
            if (col.transform.parent.GetComponent<PlayerTalkResult>().GetResult() == 1)
            {
                BePushed();
            }
        }

        //Pushes the tree, channing it's sprite and if reached the correct counter
        //Updates game flow of the players progress 
        private void BePushed()
        {
            //Checks if been pushed, if so pause pushing for 1.5 sec
            if (_pushed) return;
            _pushed = true;
            StartCoroutine(PushedTimer());
        
            //Check if the counter has reached it's max otherwise increment and update sprite 
            if (_currentSprite >= 2) return;
            _currentSprite++;
            print(_currentSprite);
            SpriteRenderer.sprite = sprites[_currentSprite];
        
            //If counter is right amount update Game Flow 
            if (_currentSprite == 2) { _gameFlow.AddQuest(GameFlow.Quests.Tree); }
        }

        //Used to reset the push
        private IEnumerator PushedTimer()
        {
            yield return new WaitForSeconds(1.5f);
            _pushed = false;
        }
    }
}
