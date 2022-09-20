using UnityEngine;


public class Tree : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    public int _currentSprite = 0;
    private bool _pushed;
    private GameFlow _gameFlow;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Random.Range(0, 2) == 1 ? new Vector3(-1,1,1) : new Vector3(1,1,1);
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _gameFlow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        _animator.Play($"Shake");
        switch (col.transform.parent.GetComponent<PlayerTalkResult>().GetResult())
        {
            case 1:
            {
                BePushed();
                break;
            }
        }
    }

    private void BePushed()
    {
        if (!_pushed)
        {
            _pushed = true;
            Invoke($"PushedTimer", 1.5f);
            if (_currentSprite >= 2) return;
            _currentSprite++;
            _spriteRenderer.sprite = sprites[_currentSprite];
            if (_currentSprite == 2)
            {
                _gameFlow.AddTree();
            }
        }
    }

    private void PushedTimer()
    {
        _pushed = false;
    }
}
