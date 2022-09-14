using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeDeath : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Invoke($"Death", 1.1f);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
