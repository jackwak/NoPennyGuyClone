using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class House : MonoBehaviour
{
    [SerializeField] private GameObject _houseScene;
    [SerializeField] private int _houseIndex;
    private CinemachineVirtualCamera camera;
    [SerializeField] private int _levelCount = 5;
    public int LastOpenedLevelIndex = 0;
    public bool IsOpen;
    // levelların datalarını tut
    public Level[] Levels;

    public GameObject GetHouseScene { get { return _houseScene; } }
    public int GetHouseIndex { get => _houseIndex; }


    private void Start()
    {
        camera = transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
    }

    public void IncreaseCameraPriority()
    {
        camera.Priority++;
    }

    public void DecreaseCameraPriority()
    {
        camera.Priority--;
    }
}
