using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class House : MonoBehaviour
{
    [SerializeField] public GameObject _houseScene;
    public int _houseIndex;
    private CinemachineVirtualCamera camera;
    public int LevelCount = 5;
    public int LastOpenedLevelIndex = 0; // bunu playerprefle tutman gerekebilir
    [HideInInspector] public bool IsOpen;
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
