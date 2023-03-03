using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    private Vector2 velocity;
    private float speed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        var random_x = Random.Range(-1.0f, 1.0f);
        var random_y = Random.Range(-1.0f, 1.0f);
        velocity = new Vector2(random_x, random_y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(velocity.y, 0.0f, velocity.x) * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;
        if (other.CompareTag("WallWest") || other.CompareTag("WallEast"))
        {
            velocity.y *= -1;
        }
        else if (other.CompareTag("WallNorth") || other.CompareTag("WallSouth"))
        {
            velocity.x *= -1;
        }
    }
}
