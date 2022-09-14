using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float _x;
    private float _y;
    [SerializeField] private float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void UpdateWalk()
    {
        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");

        _rigidbody2D.velocity = new Vector2(_x * speed, _y * speed);
    }
}
