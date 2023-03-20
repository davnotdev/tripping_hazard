using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    private Vector2 velocity;
    private float speed = 2.0f;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

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
        rigidbody.velocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector3(velocity.y, 0.0f, velocity.x) * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            //  Get wall rotation.
            var theta = other.transform.rotation.eulerAngles.y + 180;

            //  Get the normalized vector sticking out of the wall.
            var direction = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)).normalized;

            //  If the object is not facing the wall, flip the wall's normal vector.
            if (Vector3.Dot(velocity, direction) <= 0)
            {
                direction = -direction;
            }

            //  Translate the this.velocity vector to 3d space.
            var velocity_3d = new Vector3(velocity.x, 0, velocity.y);

            //  Bounce off the wall using the wall's normal vector.
            velocity_3d = Vector3.Reflect(velocity_3d, direction);

            //  Store it back.
            velocity.x = velocity_3d.x;
            velocity.y = velocity_3d.z;
            velocity = velocity.normalized;
        }
    }
}
