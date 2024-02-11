using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waiter"))
        {
            transform.parent.GetComponent<FoodPlace>().GiveTheFoodToCustomer(other);
        }
    }
}
