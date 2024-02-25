using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [HideInInspector]
    public bool isFoodOnTable;
    private int _taskFoodCount;
    private int _counter;

    private void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        //ikinci gameobjeyi spawnlamıyo
        _taskFoodCount = LevelManager.Instance._taskFoods.Count;
        _counter = _counter % _taskFoodCount;

        Instantiate(LevelManager.Instance._taskFoods[_counter].Prefab, transform);
        
        isFoodOnTable = true;
        _counter++;
    }

    public IEnumerator DelaySpawnFood(float time)
    {
        yield return new WaitForSeconds(time);

        SpawnFood();
    }
}
