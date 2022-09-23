using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject[] preFab;
    private readonly Image[] _queueImages = new Image[3];
    private Image _queueBar;
    [SerializeField] private Transform _shapeSpawn;
    private bool _hasPlayed;
    private Animator _queueAnimator;
    private GameObject _triggerCollider;
    private bool _isCountingDown;
    private float _currentTime = 10.0f;
    [SerializeField] private float waitTime = 10.0f;
    private bool _isCheatSheet;
    private List<int> _talkQueue = new List<int>();
    private GameObject _cheatSheetCanvas;
    private Animator _cameraAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        FindAndPopulateQueueImages();
        _queueBar = GameObject.Find("GameCanvas").transform.Find("QueueBar").GetComponent<Image>();
        _queueAnimator = GameObject.Find("GameCanvas").transform.Find("Queue").GetComponent<Animator>();
        _queueBar.fillAmount = 0;
        _cameraAnimator = GetComponent<Animator>();
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
    public void Update()
    {

            if (_talkQueue.Count == 3)
            {
                FullMove();
                return;
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

    private void FullMove()
    {
        if (_talkQueue.Count != 3) return;
        if ((_talkQueue[0] + 1) * (_talkQueue[1] + 1) * (_talkQueue[2] + 1) == 6)
        {
            //_cameraAnimator.Play("End");
            Invoke($"GoToLevel", 1.5f);
        }
        StartCoroutine(ResetQueue(false));
    }

    private void GoToLevel()
    {
        SceneManager.LoadScene("Level");
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
