using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTaker : MonoBehaviour
{
    [SerializeField][HideInInspector]
    public Transform WaiterFoodPosition;
    [HideInInspector]
    public bool IsFoodOnHand = false;

    private void Start()
    {
        WaiterFoodPosition = transform.Find("Food Take Position").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Food Spawner")
        {
            GameObject foodSpawnerGO = other.gameObject;
            FoodSpawner foodSpawner = foodSpawnerGO.GetComponent<FoodSpawner>();

            // take food
            if (foodSpawner.isFoodOnTable && !IsFoodOnHand)
            {
                GameObject food = foodSpawnerGO.transform.GetChild(0).gameObject;

                food.transform.SetParent(WaiterFoodPosition);

                food.transform.localPosition = Vector3.zero;

                foodSpawner.isFoodOnTable = false;
                IsFoodOnHand = true;

                StartCoroutine(foodSpawner.DelaySpawnFood(10f));
            }
        }
    }

}
