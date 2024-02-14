using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [HideInInspector]
    public bool isFoodOnTable;


    private void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        int r = Random.Range(0, LevelManager.Instance.TaskFoodGO.Count);

        Instantiate(LevelManager.Instance._taskFoods[r].Prefab, transform);
        
        isFoodOnTable = true;
    }

    public IEnumerator DelaySpawnFood(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnFood();
    }
}
