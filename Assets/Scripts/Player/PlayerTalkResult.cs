using UnityEngine;

namespace Player
{
    /// <summary>
    /// Execute the results of the player song 
    /// </summary>
    public class PlayerTalkResult : MonoBehaviour
    {
        //==================================================================================================================
        // Variables 
        //==================================================================================================================
        
        [SerializeField] private GameObject item;   //The Game Object the player instantiate 
        private string _itemName;                   //Name of the object 
        private static bool _isHoldingItem;         //If the player is holding an object 
        private SpriteRenderer _spriteRenderer;     //Where the is displayed 
        
        private int _result;                        //Last song result 
        private bool _isTransformed;                //Is player currently in leaf form 

        //==================================================================================================================
        // Methods  
        //==================================================================================================================
    
        //Connect to object and make it invisible   
        private void Start()
        {
            _spriteRenderer = transform.Find("PickedUpObject").transform.Find("Object").GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
        }

        //Directly used by Player Talk script, either drops item or transforms  
        public void PlayerAction(int result)
        {
            //Saves the result and performs the given action 
            _result = result;
            switch (result)
            {
                case 4:
                    DropItem();
                    break;
                case 12:
                {
                    Transform();
                    break;
                }
            }
        }
    
        //Return the last result from the player song 
        public int GetResult(){ return _result; }

        //================= Result = 4
        //Make the player drop the object
        private void DropItem()
        {
            if (!_isHoldingItem) return;
            //Creates the object 
            var instance = Instantiate(item, transform.position, Quaternion.identity);
            //Passes the data to that object 
            instance.GetComponent<PickUpObject>().SetNameAndSprite(_itemName, _spriteRenderer.sprite);
            
            Drop();
        }

        //================= Result 8
        //Usd by NPC to take the item from NPC 
        public void PickItem(string itemName, Sprite itemSprite)
        {
            if (_isHoldingItem) return;
            _itemName = itemName;
            _spriteRenderer.sprite = itemSprite;
            _isHoldingItem = true;
            _spriteRenderer.enabled = true;
        }

        //================= Result 12
        //Changes the player to leaf
        private void Transform() { _isTransformed = !_isTransformed; }

        //Returns to us the state of transformation 
        public bool GetIsTransformed() { return _isTransformed; }

        //================= Result 18
        //Returns the sprite player is holding to NPC 
        public Sprite GiveItemSprite() { return _spriteRenderer.sprite; }

        //Returns the name of the item player is holding to NPC
        public string GiveItemName() { return _itemName; }

        //Makes player let go of the object they were holding 
        public void Drop()
        {
            if (!_isHoldingItem) return;
            _isHoldingItem = false;
            _spriteRenderer.enabled = false;
        }
    }
}
