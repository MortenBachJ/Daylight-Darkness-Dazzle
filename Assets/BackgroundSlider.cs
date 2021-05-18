using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSlider : MonoBehaviour
{
    private BoxCollider2D collider;
    public Transform camera;
    public float width;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (camera.position.x > transform.position.x+width)
        {
            Vector2 resetPosition = new Vector2(width * 2f, 0);
            transform.position = (Vector2)transform.position + resetPosition;
        }
    }
}
