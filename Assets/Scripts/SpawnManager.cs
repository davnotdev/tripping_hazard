using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject spawn_plane;
    public GameObject enemy_prefab;
    private BoxCollider spawn_collider;
    private float interval_time = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0.0f, interval_time);
        spawn_collider = spawn_plane.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy()
    {
        var xb = spawn_collider.size.x;
        var zb = spawn_collider.size.z;

        var random_x = Random.Range(-xb, xb);
        var random_z = Random.Range(-zb, zb);

        var enemy_position = new Vector3(
            random_x,
            0.2f,
            random_z
        );

        Instantiate(enemy_prefab, enemy_position, Quaternion.identity);
    }
}
