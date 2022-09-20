using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float _x;
    private float _y;
    [SerializeField] private float speed = 2;
    [SerializeField] private float transformedSpeed = 5;
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer _spriteRenderer;

    private PlayerTalkResult _playerTalkResult;
 
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _playerTalkResult = GetComponent<PlayerTalkResult>();
    }

    // Update is called once per frame
    public void UpdateWalk()
    {
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");

        if (_playerTalkResult.GetIsTransformed())
        {
            _spriteRenderer.sprite = sprites[4];
        }
        else
        {
            switch (_x)
            {
                case > 0:
                    _spriteRenderer.sprite = sprites[0];
                    break;
                case < 0:
                    _spriteRenderer.sprite = sprites[1];
                    break;
                default:
                {
                    _spriteRenderer.sprite = _y > 0 ? sprites[2] : sprites[3];
                    break;
                }
            }
        }

        var trueSpeed = _playerTalkResult.GetIsTransformed() ? transformedSpeed : speed; 
        _rigidbody2D.velocity = new Vector2(_x * trueSpeed, _y * trueSpeed);
    }
}
