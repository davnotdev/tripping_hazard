using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject plane;
    private BoxCollider plane_collider;
    private const float travel_speed = 0.2f;
    private const uint save_tick_resolution = 10;
    private uint save_tick = 0;

    //  (previous position, normalized direction)
    private Stack<(Vector3, Vector2)> past_travels;

    void Start()
    {
        past_travels = new Stack<(Vector3, Vector2)>();
        past_travels.Push((transform.position, Vector2.zero));
        plane_collider = plane.GetComponent<BoxCollider>();
    }

    void Update()
    {
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButton(0) && plane_collider.Raycast(ray, out hit, 1000))
        {
            Vector2 travel =
                (new Vector2(hit.point.x, hit.point.z) - new Vector2(transform.position.x, transform.position.z))
                .normalized * travel_speed;
            transform.Translate(new Vector3(travel.x, 0, travel.y));

            if (save_tick == save_tick_resolution)
            {
                past_travels.Push((transform.position, travel));
                save_tick = 0;
            }

            save_tick += 1;
        }

        if (Input.GetMouseButton(1))
        {
            if (past_travels.Count > 0)
            {
                var (position, direction) = past_travels.Peek();
                transform.Translate(-new Vector3(direction.x, 0, direction.y).normalized * travel_speed);
                if (past_travels.Count != 1 && VectorHasPassedPosition(position, direction, transform.position))
                {
                    past_travels.Pop();
                }

            }
        }
    }

    bool VectorHasPassedPosition(Vector3 position, Vector2 direction, Vector3 current_position)
    {
        Vector3 diff = current_position - position;
        return Vector3.Dot(diff, new Vector3(direction.x, 0, direction.y)) < 0;
    }
}
