using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct TravelBacktrack
{
    public Vector3 position { get; }
    public GameObject joint_object { get; }
    public Joint joint { get; }

    public TravelBacktrack(Vector3 position_, GameObject joint_object_)
    {
        position = position_;
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
    private const uint save_tick_resolution = 12;
    private uint save_tick = 0;
    private Vector3 normalized_tangental_direction;
    private bool has_hit_wall = false;
    private Vector3 has_hit_wall_direction;
    private Stack<TravelBacktrack> past_travels;

    void Start()
    {
        plane_collider = plane.GetComponent<BoxCollider>();
        past_travels = new Stack<TravelBacktrack>();

        var initial_spawn_position = new Vector3(9999, 0, 0);
        var initial_joint = Instantiate(joint_prefab, initial_spawn_position, joint_prefab.transform.rotation, transform);
        past_travels.Push(new TravelBacktrack(transform.position, initial_joint));
    }

    void FixedUpdate()
    {
        //  Left Click: Move Forward.
        if (Input.GetMouseButton(0))
        {
            MoveForward();
        }

        //  Right Click: Move Backward.
        if (Input.GetMouseButton(1))
        {
            MoveBackward();
        }

        //  Update joint end.
        var travel = past_travels.Peek();
        travel.joint.end = transform.position;
    }

    void MoveForward()
    {
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (plane_collider.Raycast(ray, out hit, 1000))
        {
            Vector2 travel =
                (new Vector2(hit.point.x, hit.point.z) - new Vector2(transform.position.x, transform.position.z))
                .normalized * travel_speed;

            var travel_3d = new Vector3(travel.x, 0, travel.y);
            var normalized_direction = travel_3d.normalized;

            if (has_hit_wall && Vector3.Dot(normalized_direction, normalized_tangental_direction) > 0)
            {
                return;
            }

            has_hit_wall = false;

            normalized_tangental_direction = normalized_direction;
            var travel_location = transform.position + travel_3d;

            transform.Translate(travel_3d);

            if (save_tick == save_tick_resolution)
            {
                var initial_spawn_position = new Vector3(9999, 0, 0);
                var joint = Instantiate(joint_prefab, initial_spawn_position, joint_prefab.transform.rotation, transform);
                past_travels.Push(new TravelBacktrack(transform.position, joint));
                save_tick = 0;
            }

            save_tick += 1;
        }
    }

    void MoveBackward()
    {
        var travel = past_travels.Peek();
        var travel_diff = (transform.position - travel.position).normalized;
        transform.Translate(-travel_diff * travel_speed);
        if (past_travels.Count != 1 && VectorHasPassedPosition(travel.position, travel_diff, transform.position))
        {
            Destroy(travel.joint_object);
            past_travels.Pop();
        }
    }

    bool VectorHasPassedPosition(Vector3 position, Vector2 direction, Vector3 current_position)
    {
        Vector3 diff = current_position - position;
        return Vector3.Dot(diff, new Vector3(direction.x, 0, direction.y)) <= 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            has_hit_wall = true;
            has_hit_wall_direction = normalized_tangental_direction;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            has_hit_wall = false;
        }
    }
}
