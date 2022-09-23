using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTalk : MonoBehaviour
{
    [SerializeField] private GameObject[] preFab;
    [SerializeField] private Sprite[] sprites;
    private readonly Image[] _queueImages = new Image[3];
    private Image _queueBar;
    private Transform _shapeSpawn;
    private bool _hasPlayed;
    private Animator _queueAnimator;
    private GameObject _triggerCollider;
    private PlayerTalkResult _playerTalkResult;
    private bool _isCountingDown;
    private float _currentTime = 10.0f;
    [SerializeField] private float waitTime = 10.0f;
    private bool _isCheatSheet;
    private List<int> _talkQueue = new List<int>();
    private GameObject _cheatSheetCanvas; 
    
    // Start is called before the first frame update
    private void Start()
    {
        FindAndPopulateQueueImages();
        _shapeSpawn = transform.Find("ShapeSpawn").transform;
        _queueBar = GameObject.Find("GameCanvas").transform.Find("QueueBar").GetComponent<Image>();
        _queueAnimator = GameObject.Find("GameCanvas").transform.Find("Queue").GetComponent<Animator>();
        _cheatSheetCanvas = GameObject.Find("CheatSheetCanvas").gameObject;
        _cheatSheetCanvas.SetActive(false);
        _triggerCollider = transform.Find("Collider").gameObject;
        _triggerCollider.SetActive(false);
        _queueBar.fillAmount = 0;
        _playerTalkResult = GetComponent<PlayerTalkResult>();
    }

    private void FindAndPopulateQueueImages()
    {
        var images = GameObject.Find("GameCanvas").transform.Find("Queue");
        for (var i = 0; i < images.childCount; i++)
        {
            _queueImages[i] = images.GetChild(i).GetComponent<Image>();
        }
    }

    // Update is called once per frame
    public void UpdateTalk()
    {
        if (_isCheatSheet)
        {
            if (Input.GetKeyDown(KeyCode.Escape)){
                _isCheatSheet = false;
                _cheatSheetCanvas.SetActive(_isCheatSheet);
            }
            
        }
        else if (_playerTalkResult.GetIsTransformed())
        {
            if ((Input.GetButtonDown($"Sound1") || (Input.GetButtonDown($"Sound2")) || (Input.GetButtonDown($"Sound3"))))
            {
                _playerTalkResult.PlayerAction(12);
            }
            
            if (Input.GetKeyDown(KeyCode.Escape)){
                _isCheatSheet = true;
                _cheatSheetCanvas.SetActive(_isCheatSheet);
            }
        }
        else
        {
            if (_talkQueue.Count == 3)
            {
                FullMove();
                return;
            }
        
            if (Input.GetKeyDown(KeyCode.Escape)){
                _isCheatSheet = true;
                _cheatSheetCanvas.SetActive(_isCheatSheet);
            }
            
            if (Input.GetButtonDown($"Sound1") && !_hasPlayed)
            {
                Action(0);
            }
        
            if (Input.GetButtonDown($"Sound2") && !_hasPlayed)
            {
                Action(1);
            }
        
            if (Input.GetButtonDown($"Sound3") && !_hasPlayed)
            {
                Action(2);
            }

            UpdatingQueueCountDown();   
        }
    }

    private void FullMove()
    {
        if (_talkQueue.Count != 3) return;
        _playerTalkResult.PlayerAction((_talkQueue[0] + 1) * (_talkQueue[1] + 1) * (_talkQueue[2] + 1));
        StartCoroutine(Collider());
        StartCoroutine(ResetQueue(false));
    }

    private IEnumerator Collider()
    {
        _triggerCollider.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _triggerCollider.SetActive(false);
    }

    private void Action(int i)
    {
        UpdateQueue(i);
        var instantiate = Instantiate(preFab[i], transform.position, Quaternion.identity);
        instantiate.transform.parent = _shapeSpawn;
        _hasPlayed = true;
        _isCountingDown = true;
        _currentTime = waitTime;
        _queueBar.fillAmount = 1;
        StartCoroutine(_talkQueue.Count == 3 ? PlayTimer(1f) : PlayTimer(0.3f));
    }

    private void UpdateQueue(int i)
    {
        _talkQueue.Add(i);
        _queueImages[_talkQueue.Count - 1].enabled = true;
        _queueImages[_talkQueue.Count - 1].sprite = sprites[i];
        _queueAnimator.Play($"Queue_" + _talkQueue.Count);
    }

    private void UpdatingQueueCountDown()
    {
        if (!_isCountingDown) return;
        _currentTime -= Time.deltaTime;
        _queueBar.fillAmount -= 1.0f / waitTime * Time.deltaTime;
        if (!(_currentTime <= 0)) return;
        _hasPlayed = true;
        StartCoroutine(PlayTimer(1f));
        StartCoroutine(ResetQueue(true));
    }

    private IEnumerator ResetQueue(bool fail)
    {
        _talkQueue.Clear();
        _isCountingDown = false;
        _queueBar.fillAmount = 0;
        yield return new WaitForSeconds(0.6f);
        _queueAnimator.Play(fail ? $"Fail" : $"Full");
        yield return new WaitForSeconds(0.3f);
        foreach (var image in _queueImages)
        {
            image.enabled = false;
        }
    }

    private IEnumerator PlayTimer(float time)
    {
        yield return new WaitForSeconds(time);
        _hasPlayed = false;
    }
    
}
