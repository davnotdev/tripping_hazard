using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct TravelBacktrack
{
    public Vector3 position { get; }
    public Vector2 normalized_direction { get; }
    public GameObject joint_object { get; }
    public Joint joint { get; }

    public TravelBacktrack(Vector3 position_, Vector2 normalized_direction_, GameObject joint_object_)
    {
        position = position_;
        normalized_direction = normalized_direction_;
        joint_object = joint_object_;
        joint = joint_object.GetComponent<Joint>();
        joint.begin = position_;
        joint.end = position_;
    }
}

public class PlayerController : MonoBehaviour
{
    public GameObject plane;
    public GameObject joint_prefab;

    private BoxCollider plane_collider;
    private const float travel_speed = 0.1f;
    private const uint save_tick_resolution = 5;
    private uint save_tick = 0;

    private Stack<TravelBacktrack> past_travels;

    void Start()
    {
        plane_collider = plane.GetComponent<BoxCollider>();
        past_travels = new Stack<TravelBacktrack>();

        var initial_joint = Instantiate(joint_prefab);
        past_travels.Push(new TravelBacktrack(transform.position, Vector2.zero, initial_joint));
    }

    void Update()
    {
        //  Left Click: Move Forward.
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
                var joint = Instantiate(joint_prefab);
                past_travels.Push(new TravelBacktrack(transform.position, travel, joint));
                save_tick = 0;
            }

            save_tick += 1;
        }

        //  Right Click: Move Backward.
        if (Input.GetMouseButton(1))
        {
            var travel = past_travels.Peek();
            transform.Translate(-new Vector3(travel.normalized_direction.x, 0, travel.normalized_direction.y).normalized * travel_speed);
            if (past_travels.Count != 1 && VectorHasPassedPosition(travel.position, travel.normalized_direction, transform.position))
            {
                Destroy(travel.joint_object);
                past_travels.Pop();
            }
        }

        //  Update joint end.
        {
            var travel = past_travels.Peek();
            travel.joint.end = transform.position;
        }
    }

    bool VectorHasPassedPosition(Vector3 position, Vector2 direction, Vector3 current_position)
    {
        Vector3 diff = current_position - position;
        return Vector3.Dot(diff, new Vector3(direction.x, 0, direction.y)) < 0;
    }
}
