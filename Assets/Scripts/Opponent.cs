using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public string opponentType;
    public float movementSpeed;
    private Vector2 dir = Vector2.left;
    private Vector2 dir1 = Vector2.down;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(opponentType.Equals("Basic"))
        {

        }

        if(opponentType.Equals("GK"))
        {
            transform.Translate(dir * movementSpeed * Time.deltaTime);
            if (transform.position.x <= -1)
            {
                dir = Vector2.right;
            }
            else if (transform.position.x >= 1)
            {
                dir = Vector2.left;
            }
        }

        if(opponentType.Equals("Tackler"))
        {
            transform.Translate(dir1 * movementSpeed * Time.deltaTime);
            if (transform.position.y <= -0.02)
            {
                dir1 = Vector2.up;
            }
            else if (transform.position.y >= 1.6)
            {
                dir1 = Vector2.down;
            }
            //transform.position = new Vector2(transform.position.x, transform.position.y - movementSpeed * Time.deltaTime);
        }
    }
}
