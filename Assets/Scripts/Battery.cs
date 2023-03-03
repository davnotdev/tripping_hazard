using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private int battery_percentage = 100;
    private bool player_is_charging = false;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("BatteryTick", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_is_charging = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        player_is_charging = false;
    }

    private void BatteryTick()
    {
        if (battery_percentage <= 0)
        {
            //  TODO: Lose Condition.
            Debug.Log("You suck!");
            return;
        }

        if (player_is_charging)
        {
            battery_percentage += 1;
        }
        else
        {
            battery_percentage -= 1;
        }
    }
}
