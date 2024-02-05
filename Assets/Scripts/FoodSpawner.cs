using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waiter"))
        {
            // give the hand of waiter

            //destroy 0. index
        }
    }
}
