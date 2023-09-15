using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float dash;
    [SerializeField, Range(0,1)] private float damper;

    private Vector2 direction;
    private Vector2 saved_direction;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction = new Vector2(0.0f, 0.0f);
        bool keypressed = false;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction += new Vector2(1.0f, 0.0f);
            keypressed = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction += new Vector2(-1.0f, 0.0f);
            keypressed = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction += new Vector2(0.0f, 1.0f);
            keypressed = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction += new Vector2(0.0f, -1.0f);
            keypressed = true;
        }

        direction = direction.normalized;
        if (keypressed) {
            saved_direction = direction;
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity += saved_direction * dash * Time.deltaTime;
        }

        rb.velocity *= damper;
        rb.velocity += direction * speed * Time.deltaTime;        
    }
}
