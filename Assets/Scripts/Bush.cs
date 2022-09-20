using System;
using UnityEngine;

public class Bush : MonoBehaviour
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private GameObject preFab;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _broken;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _animator.Play($"Shake");
            if (!_broken && col.transform.parent.GetComponent<PlayerTalkResult>().GetResult() == 1)
            {
                _broken = true;
                _spriteRenderer.sprite = sprite;
                var vector = transform.position + 2 * Vector3.down;
                Instantiate(preFab, vector, Quaternion.identity);
            }
        }
    }
}
