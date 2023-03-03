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
        var random_theta = Random.Range(45 - 10, 45 + 10);
        if (Random.Range(0.0f, 0.5f) < 0.5f)
        {
            random_theta *= -1;
        }

        if (Random.Range(0.0f, 0.5f) < 0.5f)
        {
            random_theta += 180;
        }

        velocity = new Vector2(Mathf.Cos(random_theta), Mathf.Sin(random_theta));
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
